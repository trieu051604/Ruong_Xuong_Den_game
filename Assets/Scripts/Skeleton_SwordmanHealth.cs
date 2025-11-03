using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 60;     // Maximum HP of enemy
    private int currentHealth;     // Current HP
    public Slider healthBar;       // Health bar UI

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Called when enemy takes damage from player
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Update the enemy's health bar fill
    void UpdateHealthBar()
    {
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;
    }

    // Handle enemy death
    void Die()
    {
        Debug.Log("Enemy (Boss) defeated!");
        // Add death animation, drop loot, disable collider, etc.
        Destroy(gameObject, 0.5f);
    }
}
