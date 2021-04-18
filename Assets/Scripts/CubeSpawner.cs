using UnityEngine;

public partial class CubeSpawner : MonoBehaviour
{
    [SerializeField] private MovingCube cubePrefab;
    [SerializeField] private MoveDirection moveDirection;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.PrevCube != null && MovingCube.PrevCube.gameObject != GameObject.Find("StartingCube")) 
        {
            float xPos = moveDirection == MoveDirection.X ? transform.position.x : MovingCube.PrevCube.transform.position.x;
            float zPos = moveDirection == MoveDirection.Z ? transform.position.z : MovingCube.PrevCube.transform.position.z;

            cube.transform.position = new Vector3(
                xPos,
                MovingCube.PrevCube.transform.position.y + cubePrefab.transform.localScale.y,
                zPos
            );
        }
        else
        {
            cube.transform.position = transform.position;
        }

        cube.MoveDirection = moveDirection;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.transform.position, cubePrefab.transform.localScale);
    }
}
