using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }

    public static MovingCube PrevCube { get; private set; }

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

    [SerializeField] float moveSpeed = 1f;

    int prevColour;

    private void OnEnable()
    {
        if (PrevCube == null)
        {
            PrevCube = this;
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
        float hangoverAmount = transform.position.z - PrevCube.transform.position.z;

        if (Mathf.Abs (hangoverAmount) >= PrevCube.transform.localScale.z)
        {
            PrevCube = null;
            CurrentCube = null;
            SceneManager.LoadScene(0);
        }

        float direction = hangoverAmount > 0 ? 1f : -1f;
        SplitCubeOnZ(hangoverAmount, direction);

        PrevCube = this;
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
    }

    private void SpawnDropCube(float zPos, float zSize)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3 (transform.localScale.x, transform.localScale.y, zSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, zPos);

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<Renderer>().material = gameObject.GetComponent<Renderer>().material;
        Destroy(cube.gameObject, 2f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
}