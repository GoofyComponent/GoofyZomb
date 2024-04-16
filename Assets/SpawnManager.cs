using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] zombies;
    public int maxWaveZombies = 300; // Nombre maximal de zombies par wave
    public int maxZombies = 20; // Nombre maximal de zombies simultanés
    public float spawnInterval = 5f; // Intervalle de spawn en secondes
    private float spawnTimer = 0f; // Timer de spawn
    private int wave = 1;
    private int currentZombies = 0; // Nombre de zombies actuellement en jeu
    private int waveZombiesReamining = 0; // Nombre de zombies restants de la wave actuelle
    private int zombieSpawned = 0; // Nombre de zombies spawnés
    private bool waveComplete = false; // Indicateur de fin de wave



    // Start is called before the first frame update
    void Start()
    {
        waveZombiesReamining = getNumberOfZombiePerWave(wave);
    }

    // Update is called once per frame
    void Update()
    {
        // if the player presses the b, finish the wave
        if (Input.GetKeyDown(KeyCode.B))
        {
            FinishWave();
        }

        print("Wave: " + wave + " Zombies: " + currentZombies + " Remaining: " + waveZombiesReamining);
        // update the spawn timer 
        if (spawnTimer  < spawnInterval )
        {
            spawnTimer += Time.deltaTime;
            return ;
        }
        else
        {
            spawnTimer = 0f;
        }
        



        // if there are less than the max zombies, spawn a new zombie
        if (currentZombies < maxZombies && waveZombiesReamining > 0 && zombieSpawned < getNumberOfZombiePerWave(wave))
        {
            SpawnZombie();
        }
        else if (waveZombiesReamining == 0)
        {
            waveComplete = true;
        }



        // if the wave is complete, start the next wave
        if (waveComplete)
        {
            waveComplete = false;
            zombieSpawned = 0;
            currentZombies = 0;
            wave++;
            waveZombiesReamining = getNumberOfZombiePerWave(wave);
        }
            
    }

    // function to finish the wave
    private void FinishWave()
    {
        // find all tag with "zombie_entity" and destroy them
        GameObject[] zombies = GameObject.FindGameObjectsWithTag("zombie_entity");
        foreach (GameObject zombie in zombies)
        {
            Destroy(zombie);
        }
        waveZombiesReamining = 0;
        waveComplete = true;
        zombieSpawned = 0;
        currentZombies = 0;
    }


    // function to calculate a the no. of zombies in a wave
    private int getNumberOfZombiePerWave(int n) {
        int waveZombies = Mathf.RoundToInt((n * (n + 1)) / 2);
        return Mathf.Min(waveZombies, maxWaveZombies);
    }

    // function to spawn a zombie
    private void SpawnZombie()
    {
        // get a random spawn point
        int spawnPointIndex = Random.Range(1, spawnPoints.Length + 1);
        GameObject spawnPoint = spawnPoints[spawnPointIndex - 1];

        // get a random zombie
        int zombieIndex = Random.Range(0, zombies.Length);
        GameObject zombie = zombies[0];

        // spawn the zombie
        Instantiate(zombie, spawnPoint.transform.position, spawnPoint.transform.rotation);
        currentZombies++;
        zombieSpawned++;
    }

    // function to get the current wave
    public int getWave()
    {
        return wave;
    }

    public void removeOneZombie(){
        waveZombiesReamining = waveZombiesReamining -1;
    }
}
