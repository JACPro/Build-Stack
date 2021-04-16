using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    public static MovingCube CurrentCube { get; private set; }

    public static MovingCube PrevCube { get; private set; }
    
    [SerializeField] float moveSpeed = 1f;

    private void OnEnable()
    {
        if (PrevCube == null)
        {
            PrevCube = this;
        }
        CurrentCube = this;

        GetComponent<Renderer>().material.color = GetRandomColor();
    }

    private Color GetRandomColor()
    {
        //Colour range = 10, 20... 100
        float hue = (UnityEngine.Random.Range(10f, 100f)) / 10f;
        hue = (Mathf.Round(hue) * 10f) / 100f;

        Debug.Log(hue);

        return Color.HSVToRGB(
            hue, 
            0.6f, 
            0.7f
        );
    }

    internal void Stop()
    {
        moveSpeed = 0f;
        float hangoverAmount = transform.position.z - PrevCube.transform.position.z;

        float direction = hangoverAmount > 0 ? 1f : -1f;

        SplitCubeOnZ(hangoverAmount, direction);
    }

    private void SplitCubeOnZ(float hangoverAmount, float direction)
    {
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
        Destroy(cube.gameObject, 2f);
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
}
