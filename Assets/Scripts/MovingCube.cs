using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }
    public static MovingCube PrevCube { get; private set; }
    public MoveDirection MoveDirection { get; set; }

    [SerializeField] float moveSpeed = 1f;

    Color[] colours = {
        new Color(1f, 228f/254f, 11f/254f), //yellow
        new Color(240f/255f, 116/254f, 0f), //orange
        new Color(202f/255f, 43f/254f, 39f/254f), //red
        new Color(4f/255f, 60f/254f, 97f/254f), //dark blue
        new Color(0f, 136f/254f, 196f/254f), //mid blue
        new Color(62f/255f, 196f/254f, 109f/254f), //green
        new Color(123f/255f, 87f/254f, 48f/254f), //brown
        new Color(226f/255f, 230f/254f, 230f/254f), //white
        new Color(28f/255f, 55f/254f, 84f/254f), //black
    };

    int prevColour;

    private void Start()
    {
        if (PrevCube == null)
        {
            PrevCube = GameObject.Find("StartingCube").GetComponent<MovingCube>();
        }
        CurrentCube = this;
        GetComponent<Renderer>().material.color = GetRandomColor();

        transform.localScale = new Vector3 (PrevCube.transform.localScale.x, transform.localScale.y, PrevCube.transform.localScale.z);
    }

    private Color GetRandomColor()
    {
        int randNo = UnityEngine.Random.Range(0, colours.Length - 1);

        return colours[randNo];
    }

    internal void Stop()
    {
        moveSpeed = 0f;
        float hangoverAmount = GetHangoverAmount();


        float max = MoveDirection == MoveDirection.Z ? PrevCube.transform.localScale.z : PrevCube.transform.localScale.x;
        if (Mathf.Abs(hangoverAmount) >= max)
        {
            PrevCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }

        float direction = hangoverAmount > 0 ? 1f : -1f;

        if (MoveDirection == MoveDirection.Z)
        {
            SplitCubeOnZ(hangoverAmount, direction);

        }
        else
        {
            SplitCubeOnX(hangoverAmount, direction);
        }

        PrevCube = this;
    }

    private float GetHangoverAmount()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            return transform.position.z - PrevCube.transform.position.z;
        } 
        else
        {
            return transform.position.x - PrevCube.transform.position.x;
        }
    }

    private void SplitCubeOnZ(float hangoverAmount, float direction)
    {
        if (Mathf.Abs(hangoverAmount) >= 1)
        {
            //Lose
            gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject, 2f);

            return;
        }

        float newZSize = PrevCube.transform.localScale.z - Mathf.Abs(hangoverAmount);
        float fallingBlockSize = transform.localScale.z - newZSize;

        transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, newZSize);

        float newZPos = PrevCube.transform.position.z + (hangoverAmount / 2);

        transform.position = new Vector3 (transform.position.x, transform.position.y, newZPos);

        float cubeEdge = transform.position.z + (newZSize / 2f  * direction);
        float fallingBlockZPos = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockZPos, fallingBlockSize);

        PrevCube = this;
    }

    private void SplitCubeOnX(float hangoverAmount, float direction)
    {
        if (Mathf.Abs(hangoverAmount) >= 1)
        {
            //Lose
            gameObject.AddComponent<Rigidbody>();
            Destroy(gameObject, 2f);

            return;
        }

        float newXSize = PrevCube.transform.localScale.x - Mathf.Abs(hangoverAmount);
        float fallingBlockSize = transform.localScale.x - newXSize;

        transform.localScale = new Vector3(newXSize, transform.localScale.y, transform.localScale.z);

        float newXPos = PrevCube.transform.position.x + (hangoverAmount / 2);

        transform.position = new Vector3(newXPos, transform.position.y, transform.position.z);

        float cubeEdge = transform.position.x + (newXSize / 2f * direction);
        float fallingBlockXPos = cubeEdge + fallingBlockSize / 2f * direction;

        SpawnDropCube(fallingBlockXPos, fallingBlockSize);

        PrevCube = this;
    }

    private void SpawnDropCube(float newPos, float newSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);

        if (MoveDirection == MoveDirection.Z) 
        {
            cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, newSize);
            cube.transform.position = new Vector3(transform.position.x, transform.position.y, newPos);    
        }
        else
        {
            cube.transform.localScale = new Vector3(newSize, transform.localScale.y, transform.localScale.z);
            cube.transform.position = new Vector3(newPos, transform.position.y, transform.position.z);
        }

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
        Destroy(cube.gameObject, 2f);
    }

    void Update()
    {
        if (MoveDirection == MoveDirection.Z)
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
        else if (MoveDirection == MoveDirection.X) 
        {
            transform.position += transform.right * Time.deltaTime * moveSpeed;
        }
    }
}