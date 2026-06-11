using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // variables
    [SerializeField] private GameObject lightPrefab;
    [SerializeField] private GameObject normalPrefab;
    [SerializeField] private GameObject tankPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float timeBetweenSpawns = 0.5f;

    private int enemiesAlive;

    public void StartWave(int waveNumber) // start a wave 
    {
        StartCoroutine(SpawnWave(waveNumber)); // based on the number
    }

    private IEnumerator SpawnWave(int waveNumber)  // co routine! i think
    {
        int enemyCount = Mathf.Min(3 + waveNumber, 25); // enemy number

        for (int i = 0; i < enemyCount; i++) // loop enemies
        {
            GameObject prefab = PickEnemy(waveNumber); // get a fella
            GameObject point = spawnPoints[Random.Range(0, spawnPoints.Length)]; // pick a random spawn point
            Instantiate(prefab, point.transform.position, Quaternion.identity); 
            enemiesAlive++; // increase amt of enemies
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private GameObject PickEnemy(int wave)
    {
        int maxTier = 0; // handles what enemies can sapwn
        if (wave >= 11) maxTier = 1;
        if (wave >= 21) maxTier = 2;
        if (wave >= 31) maxTier = 3;

        int roll = Random.Range(0, maxTier + 1);
        switch (roll) // siwitch case on what it should use
        {
            case 1: return normalPrefab;
            case 2: return tankPrefab;
            case 3: return bossPrefab;
            default: return lightPrefab;
        }
    }

    public void EnemyKilled() // if a guy dies
    {
        enemiesAlive--; // get rid of an enemy
        if (enemiesAlive <= 0) // if it is 0 
            GameManager.Instance.WaveComplete(); // complete wave
    }
}