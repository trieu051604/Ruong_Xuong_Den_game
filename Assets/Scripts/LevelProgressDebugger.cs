using UnityEngine;

/// <summary>
/// Test script to verify the enemy spawn and level progression system
/// Attach this to any GameObject in the scene to get debug information
/// </summary>
public class LevelProgressDebugger : MonoBehaviour
{
    [Header("Debug Info")]
    public bool showDebugUI = true;
    public bool logEnemyEvents = true;

    private LevelManager levelManager;

    void Start()
    {
        levelManager = Object.FindFirstObjectByType<LevelManager>();

        if (levelManager == null)
        {
            Debug.LogWarning("LevelManager not found in scene!");
        }

        if (logEnemyEvents)
        {
            EnemyHealth.OnEnemyDeath += OnEnemyDeath;
        }
    }

    void OnDestroy()
    {
        if (logEnemyEvents)
        {
            EnemyHealth.OnEnemyDeath -= OnEnemyDeath;
        }
    }

    void OnEnemyDeath(EnemyHealth.EnemyType enemyType)
    {
        Debug.Log($"[LevelProgressDebugger] Enemy died: {enemyType}");
    }

    void OnGUI()
    {
        if (!showDebugUI) return;

        GUI.Box(new Rect(10, 10, 300, 150), "Level Progress Debug");

        if (levelManager != null)
        {
            GUI.Label(new Rect(20, 30, 280, 20), $"Enemies in scene: {Object.FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None).Length}");
            GUI.Label(new Rect(20, 50, 280, 20), "Press J to attack enemies");
            GUI.Label(new Rect(20, 70, 280, 20), "Kill all Slimes & Bombschrooms first");
            GUI.Label(new Rect(20, 90, 280, 20), "Then Skeleton will spawn");
            GUI.Label(new Rect(20, 110, 280, 20), "Kill Skeleton to complete level");
        }
        else
        {
            GUI.Label(new Rect(20, 30, 280, 20), "LevelManager not found!");
            GUI.Label(new Rect(20, 50, 280, 20), "Please add LevelManager to scene");
        }

        // Manual spawn buttons for testing
        if (GUI.Button(new Rect(20, 130, 100, 20), "Test Spawn"))
        {
            if (levelManager != null)
            {
                Debug.Log("Manual spawn test - check LevelManager settings");
            }
        }
    }

    void Update()
    {
        // Quick debug shortcut
        if (Input.GetKeyDown(KeyCode.F1))
        {
            showDebugUI = !showDebugUI;
        }

        // Count enemies for debugging
        if (Input.GetKeyDown(KeyCode.F2))
        {
            EnemyHealth[] enemies = GameObject.FindObjectsByType<EnemyHealth>(FindObjectsSortMode.None);
            Debug.Log($"Current enemies in scene: {enemies.Length}");

            foreach (var enemy in enemies)
            {
                Debug.Log($"- {enemy.enemyType}: {enemy.currentHealth}/{enemy.maxHealth} HP");
            }
        }
    }
}