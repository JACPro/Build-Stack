using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnCubeSpawned = delegate { };

    private CubeSpawner[] spawners;
    private int spawnerIndex;
    private CubeSpawner currentSpawner;

    AudioSource audio;

    private void Start() 
    {
        audio = GetComponent<AudioSource>();
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
            audio.pitch = UnityEngine.Random.Range(0.7f, 1.3f);
            audio.Play();
        }
    }
}
