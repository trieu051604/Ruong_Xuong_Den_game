using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Player Stats")]
    public int maxHealth = 100;
    private int currentHealth;

    [Header("UI Health")]
    public Transform hpBar;   // HP bar transform

    private Vector3 hpOriginalScale;

    void Start()
    {
        currentHealth = maxHealth;

        if (hpBar != null)
            hpOriginalScale = hpBar.localScale;

        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateHealthBar()
    {
        if (hpBar != null)
        {
            float ratio = Mathf.Clamp01((float)currentHealth / maxHealth);
            Vector3 scale = hpOriginalScale;
            scale.x = hpOriginalScale.x * ratio;
            hpBar.localScale = scale;
        }
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    void Die()
    {
        Debug.Log("Player is dead!");

        // Stop player actions immediately
        // Disable components here if needed, e.g. movement, collider, etc.

        // Start coroutine to load GameOver scene
        StartCoroutine(LoadGameOverScene());
    }

    private IEnumerator LoadGameOverScene()
    {
        // Optional: wait one frame to render last frame
        yield return null;

        // Save the current level name so Restart button can reload it
        PlayerPrefs.SetString("LastLevel", SceneManager.GetActiveScene().name);

        // Load the GameOver scene
        SceneManager.LoadScene("GameOver");
    }

    public void HealFull()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }
}
