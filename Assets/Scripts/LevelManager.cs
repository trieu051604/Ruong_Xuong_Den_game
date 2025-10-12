using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject slimePrefab;
    public GameObject bombschroomPrefab;
    public GameObject skeletonPrefab;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public int slimesToSpawn = 5;
    public int bombschroomsToSpawn = 5;

    [Header("Level Progression")]
    public string nextLevelName = "Level2";

    private int slimesKilled = 0;
    private int bombschroomsKilled = 0;
    private bool skeletonSpawned = false;
    private bool levelCompleted = false;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    void Start()
    {
        // Subscribe to enemy death events
        EnemyHealth.OnEnemyDeath += OnEnemyDied;

        // Start spawning enemies
        SpawnInitialEnemies();
    }

    void OnDestroy()
    {
        // Unsubscribe from events
        EnemyHealth.OnEnemyDeath -= OnEnemyDied;
    }

    void SpawnInitialEnemies()
    {
        Debug.Log("Spawning initial enemies...");

        // Spawn Slimes
        for (int i = 0; i < slimesToSpawn; i++)
        {
            SpawnEnemy(slimePrefab, EnemyHealth.EnemyType.Slime);
        }

        // Spawn Bombschrooms
        for (int i = 0; i < bombschroomsToSpawn; i++)
        {
            SpawnEnemy(bombschroomPrefab, EnemyHealth.EnemyType.Bombschroom);
        }

        Debug.Log($"Spawned {slimesToSpawn} Slimes and {bombschroomsToSpawn} Bombschrooms");
    }

    void SpawnEnemy(GameObject prefab, EnemyHealth.EnemyType enemyType)
    {
        if (prefab == null)
        {
            Debug.LogWarning($"Prefab for {enemyType} is null!");
            return;
        }

        // Get random spawn point
        Vector3 spawnPosition = GetRandomSpawnPoint();

        // Instantiate enemy
        GameObject enemy = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Add EnemyHealth component if it doesn't exist
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health == null)
        {
            health = enemy.AddComponent<EnemyHealth>();
        }

        // Set enemy type and health based on type
        health.enemyType = enemyType;

        switch (enemyType)
        {
            case EnemyHealth.EnemyType.Slime:
                health.maxHealth = 50f;
                break;
            case EnemyHealth.EnemyType.Bombschroom:
                health.maxHealth = 75f;
                break;
            case EnemyHealth.EnemyType.Skeleton:
                health.maxHealth = 200f;
                break;
        }

        health.currentHealth = health.maxHealth;

        // Add to spawned enemies list
        spawnedEnemies.Add(enemy);

        Debug.Log($"Spawned {enemyType} at {spawnPosition} with {health.maxHealth} HP");
    }

    Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            return spawnPoints[randomIndex].position;
        }
        else
        {
            // Random position around the center if no spawn points are set
            float x = Random.Range(-8f, 8f);
            float y = Random.Range(-4f, 4f);
            return new Vector3(x, y, 0);
        }
    }

    void OnEnemyDied(EnemyHealth.EnemyType enemyType)
    {
        OnEnemyKilled(enemyType);
    }

    /// <summary>
    /// Public method that can be called by EnemyDamageReceiver or other scripts
    /// </summary>
    public void OnEnemyKilled(EnemyHealth.EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyHealth.EnemyType.Slime:
                slimesKilled++;
                Debug.Log($"Slime killed! ({slimesKilled}/{slimesToSpawn})");
                break;

            case EnemyHealth.EnemyType.Bombschroom:
                bombschroomsKilled++;
                Debug.Log($"Bombschroom killed! ({bombschroomsKilled}/{bombschroomsToSpawn})");
                break;

            case EnemyHealth.EnemyType.Skeleton:
                Debug.Log("Skeleton killed! Level completed!");
                CompleteLevel();
                return;
        }

        // Check if all initial enemies are killed
        if (slimesKilled >= slimesToSpawn && bombschroomsKilled >= bombschroomsToSpawn && !skeletonSpawned)
        {
            SpawnSkeleton();
        }
    }

    void SpawnSkeleton()
    {
        if (skeletonSpawned) return;

        skeletonSpawned = true;
        Debug.Log("All initial enemies killed! Spawning Skeleton...");

        // Wait a moment before spawning skeleton
        Invoke(nameof(DoSpawnSkeleton), 2f);
    }

    void DoSpawnSkeleton()
    {
        if (skeletonPrefab != null)
        {
            Vector3 spawnPosition = GetRandomSpawnPoint();
            GameObject skeleton = Instantiate(skeletonPrefab, spawnPosition, Quaternion.identity);

            // Add EnemyHealth component if it doesn't exist
            EnemyHealth health = skeleton.GetComponent<EnemyHealth>();
            if (health == null)
            {
                health = skeleton.AddComponent<EnemyHealth>();
            }

            // Set as Skeleton type with higher health
            health.enemyType = EnemyHealth.EnemyType.Skeleton;
            health.maxHealth = 200f; // Skeleton has more health
            health.currentHealth = health.maxHealth;

            // Add to spawned enemies list
            spawnedEnemies.Add(skeleton);

            Debug.Log("Boss Skeleton with 200 HP!");
        }
        else
        {
            Debug.LogWarning("Skeleton prefab is null!");
        }
    }

    void CompleteLevel()
    {
        if (levelCompleted) return;

        levelCompleted = true;
        Debug.Log("Level completed! Moving to next level...");

        // Wait a moment before loading next level
        Invoke(nameof(LoadNextLevel), 3f);
    }

    void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(nextLevelName))
        {
            SceneManager.LoadScene(nextLevelName);
        }
        else
        {
            Debug.Log("No next level specified. Game completed!");
        }
    }

    // GUI for debugging
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), $"Slimes: {slimesKilled}/{slimesToSpawn}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Bombschrooms: {bombschroomsKilled}/{bombschroomsToSpawn}");
        GUI.Label(new Rect(10, 50, 200, 20), $"Skeleton: {skeletonSpawned}");
        GUI.Label(new Rect(10, 70, 200, 20), $"Level completed: {levelCompleted}");
    }
}