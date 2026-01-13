using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("Enemy prefab to spawn")]
    public GameObject enemyPrefab;
    
    [Tooltip("Range along X axis to spawn enemies (left and right from center)")]
    public float spawnRangeX = 5f;
    
    [Header("Spawn Rate")]
    [Tooltip("Time between spawns in seconds")]
    public float spawnInterval = 2f;
    
    [Tooltip("Start spawning automatically on start")]
    public bool autoStart = true;
    
    [Header("Spawn Limits")]
    [Tooltip("Maximum number of enemies to spawn (0 = unlimited)")]
    public int maxSpawns = 0;
    
    [Header("Victory")]
    public Canvas victoryPanel;
    
    private float nextSpawnTime;
    private int spawnedCount = 0;
    private int aliveCount = 0;
    private bool isSpawning = false;

    private void Start()
    {
        if (victoryPanel != null)
        {
            victoryPanel.gameObject.SetActive(false);
        }
        
        if (autoStart)
        {
            StartSpawning();
        }
    }

    private void Update()
    {
        if (isSpawning && Time.time >= nextSpawnTime)
        {
            // Check if we've reached max spawns
            if (maxSpawns > 0 && spawnedCount >= maxSpawns)
            {
                StopSpawning();
                return;
            }

            SpawnEnemy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            // Debug.LogError("Enemy prefab is not assigned!");
            return;
        }

        // Random position along X axis
        float randomX = Random.Range(-spawnRangeX, spawnRangeX);
        Vector3 spawnPosition = transform.position + transform.right * randomX;

        // Spawn enemy with spawner's rotation
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, transform.rotation);
        spawnedCount++;
        aliveCount++;
        
        // Debug.Log($"Enemy spawned at {spawnPosition}. Total spawned: {spawnedCount}, Alive: {aliveCount}");
    }

    public void StartSpawning()
    {
        isSpawning = true;
        nextSpawnTime = Time.time + spawnInterval;
        Debug.Log("Spawner started");
    }

    public void StopSpawning()
    {
        isSpawning = false;
        Debug.Log("Spawner stopped");
    }

    public void ResetSpawner()
    {
        spawnedCount = 0;
        aliveCount = 0;
        nextSpawnTime = Time.time;
    }
    
    public void OnEnemyKilled()
    {
        aliveCount--;
        Debug.Log($"Enemy killed. Alive: {aliveCount}, Spawned: {spawnedCount}/{maxSpawns}");
        
        CheckVictory();
    }
    
    private void CheckVictory()
    {
        // Victory if all enemies spawned and none alive
        bool allSpawned = maxSpawns > 0 && spawnedCount >= maxSpawns;
        bool noneAlive = aliveCount <= 0;
        
        if (allSpawned && noneAlive)
        {
            Debug.Log("Victory!");
            if (victoryPanel != null)
            {
                victoryPanel.gameObject.SetActive(true);
            }
        }
    }
    
    public int GetAliveCount() => aliveCount;
    public int GetSpawnedCount() => spawnedCount;

    // Draw spawn range in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 leftPoint = transform.position - transform.right * spawnRangeX;
        Vector3 rightPoint = transform.position + transform.right * spawnRangeX;
        
        // Draw line showing spawn range
        Gizmos.DrawLine(leftPoint, rightPoint);
        
        // Draw spheres at ends
        Gizmos.DrawWireSphere(leftPoint, 0.2f);
        Gizmos.DrawWireSphere(rightPoint, 0.2f);
        Gizmos.DrawWireSphere(transform.position, 0.3f);
    }
}
