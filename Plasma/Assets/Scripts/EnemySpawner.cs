// UNUSED SCRIPT - THIS WOULD SPAWN IN EMEIES BUT NOW IT DOENST BECAUSE ITS NOT NEEDED OK BTYE
// -------------------------------------------------

using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // variables
    public GameObject[] spawnPoints;
    public GameObject enemyPrefab;
    public Transform player;
    public float spawnInterval = 2f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), spawnInterval, spawnInterval);
    }
 
    private void SpawnEnemy()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0 || player == null) return;
 
        GameObject point = spawnPoints[Random.Range(0, spawnPoints.Length)];
 
        GameObject enemy = Instantiate(enemyPrefab, point.transform.position, Quaternion.identity);
 
        EnemyMover mover = enemy.GetComponent<EnemyMover>();
        if (mover != null)
            mover.SetTarget(player);
    }
}
 