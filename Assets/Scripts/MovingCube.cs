using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingCube : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * moveSpeed;
    }
}
