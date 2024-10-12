using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int numberOfEnemies;     
        public float spawnRate;         
    }

    public List<Wave> waves;            
    public Transform[] spawnPoints;     
    public GameObject enemyPrefab;      
    public int currentWaveIndex = 0;    
    public float timeBetweenWaves = 5f; 
    public bool isWaveInProgress = false;  

    public int enemiesAlive;          

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (enemiesAlive == 0 && !isWaveInProgress)
        {
            GameManager.obj.TimeBetweenRounds();
        }
    }
    public void ResumeWave()
    {
        StartCoroutine(BeginNextWave());
    }
    IEnumerator BeginNextWave()
    {
        isWaveInProgress = true;
        yield return new WaitForSeconds(timeBetweenWaves);

        currentWaveIndex++;
        UIManager.obj.updateWave();
        if (currentWaveIndex < waves.Count)
        {
            Debug.Log("Nueva Oleada");
            StartNextWave();
        }
        else
        {
            Debug.Log("RondasTerminadas");
        }
    }

    void StartNextWave()
    {
        Wave wave = waves[currentWaveIndex];   
        StartCoroutine(SpawnEnemies(wave));    
    }

    IEnumerator SpawnEnemies(Wave wave)
    {
        enemiesAlive = wave.numberOfEnemies;

        for (int i = 0; i < wave.numberOfEnemies; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(wave.spawnRate);
        }

        isWaveInProgress = false;
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        enemy.GetComponent<Soldier>().OnDeath += OnEnemyDeath;  
    }

    void OnEnemyDeath()
    {
        UIManager.obj.updateWave();
        enemiesAlive--;
    }
}
