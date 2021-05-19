using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };

    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;

    AudioSource tickSFX;

    private void Start() 
    {
        tickSFX = GetComponent<AudioSource>();
        spawners = FindObjectsOfType<CubeSpawner>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (MovingCube.CurrentCube != null) {
                MovingCube.CurrentCube.Stop();
            }
            spawnerIndex = spawnerIndex == 0 ? 1 : 0;
            currentSpawner = spawners[spawnerIndex];

            currentSpawner.SpawnCube();
            OnCubeSpawned();

            //Randomise pitch of block placement tick each time
            tickSFX.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
            tickSFX.Play();
        }
    }
}
