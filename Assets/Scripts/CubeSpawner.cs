﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private MovingCube cubePrefab;

    public void SpawnCube()
    {
        var cube = Instantiate(cubePrefab);

        if (MovingCube.PrevCube != null && MovingCube.PrevCube.gameObject != GameObject.Find("StartingCube")) 
        {
            cube.transform.position = new Vector3(
                transform.position.x,
                MovingCube.PrevCube.transform.position.y + cubePrefab.transform.localScale.y,
                transform.position.z
            );
        }
        else
        {
            cube.transform.position = transform.position;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.transform.position, cubePrefab.transform.localScale);
    }
}
