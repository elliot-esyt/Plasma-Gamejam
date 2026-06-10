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

    public void StartWave(int waveNumber)
    {
        StartCoroutine(SpawnWave(waveNumber));
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        int enemyCount = Mathf.Min(3 + waveNumber, 25);

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject prefab = PickEnemy(waveNumber);
            GameObject point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(prefab, point.transform.position, Quaternion.identity);
            enemiesAlive++;
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private GameObject PickEnemy(int wave)
    {
        int maxTier = 0;
        if (wave >= 11) maxTier = 1;
        if (wave >= 21) maxTier = 2;
        if (wave >= 31) maxTier = 3;

        int roll = Random.Range(0, maxTier + 1);
        switch (roll)
        {
            case 1: return normalPrefab;
            case 2: return tankPrefab;
            case 3: return bossPrefab;
            default: return lightPrefab;
        }
    }

    public void EnemyKilled()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0)
            GameManager.Instance.WaveComplete();
    }
}