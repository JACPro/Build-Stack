using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private float camYOffset;

    void Start()
    {
        camYOffset = transform.position.y - MovingCube.CurrentCube.transform.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camVelocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(
            transform.position,
            new Vector3(
                transform.position.x,
                MovingCube.CurrentCube.transform.position.y + camYOffset,
                transform.position.z
            ),
            ref camVelocity,
            0.1f
        );
    }
}
