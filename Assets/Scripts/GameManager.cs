using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MovingCube.CurrentCube != null) {
                MovingCube.CurrentCube.Stop();
                FindObjectOfType<CubeSpawner>().SpawnCube();
            }
        }
    }
}
