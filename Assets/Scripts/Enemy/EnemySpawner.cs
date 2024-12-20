using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups; //A list of enemy groups to spawn in this wave
        public int waveQuota; //Total number of enemies to spawn in this wave
        public float spawnInterval; //Interval at which to spawn enemies
        public int spawnCount; //Number of enemies already spawned in this wave
    }

    [System.Serializable]
     public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount; //The number of enemies to spawn in this wave
        public int spawnCount; //The number of enemies of this type already spawned in this wave
        public GameObject enemyPrefab;
    }


    public List<Wave> waves; //A list of all the waves in the game
    public int currentWaveCount; //The index of the current wave

    [Header("Spawner Attributes")]
    float spawnTimer; //Timer to dettermine when to spawn the next enemy
    public int enemiesAlive; //
    public int maxEnemiesAllowed; //The maximum number of enemies allowed on the map at once
    public bool maxEnemiesReached = false; //A flag indicating that the limit has been reached
    public float waveInterval; //The interval between each wave

    [Header("Spawn Points")]
    public List<Transform> relativeSpawnPoints; //A list to store all the relative spawn points of enemies.

    // Start is called before the first frame update
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
        CalculateWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        if(currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount == 0) //CHeck if the wave has ended and the next wave should start
        {
            StartCoroutine(BeginNextWave());
        }
        spawnTimer += Time.deltaTime; //Increases spawn rate based on the passing time and checking if the spawnTimer is >= currentWaveCount
        
        //Checks if it's time to spawn the next enemy
        if (spawnTimer >= waves[currentWaveCount].spawnInterval)
        {
            spawnTimer = 0;
            SpawnEnemies();
        }
    }

    IEnumerator BeginNextWave()
    {
        yield return new WaitForSeconds(waveInterval);

        //Wave for 'waveInterval' seconds before starting the next wave
        if(currentWaveCount < waves.Count - 1)
        {
            //If there are more waves to start after the current wave, move on to the next wave
            currentWaveCount++;
            CalculateWaveQuota() ;
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        Debug.LogWarning(currentWaveQuota);
    }
    /// <summary>
    /// This method will stop spawning enemies if the amount of enemies on the map has reached the limit.
    /// This method will only spawn enemies in a particular wave until it is time for the next wave's enemies to be spawned.
    /// </summary>
    void SpawnEnemies()
    {
        //To check if the minimum number of enemies in the wave have been spawned
        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota && !maxEnemiesReached)
        {
            //Spawn each type of enemy unti lthe quota is filled
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                //To check if the minimum number of enemies in the wave have been spawned
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    //Limit the number of enemies that can be spawned at once
                    if(enemiesAlive >= maxEnemiesAllowed)
                    {
                        maxEnemiesReached = true;
                        return;
                    }

                    //Spawn the enemy at a random position close to the player
                    Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;
                }
            }
        }
        //Resets the maxEnemiesReached flag if the number of enemies alive has droped below the maximum amount
        if(enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }
    //Calls this function when an enemy is killed
    public void OnEnemyKilled()
    {
        //Decrement the number of enemies alive
        enemiesAlive--;
    }
}
