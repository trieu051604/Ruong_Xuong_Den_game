using UnityEngine;
using System;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    public float currentHealth;
    
    [Header("Enemy Type")]
    public EnemyType enemyType;
    
    public static event Action<EnemyType> OnEnemyDeath;
    
    public enum EnemyType
    {
        Slime,
        Bombschroom,
        Skeleton
    }
    
    void Start()
    {
        currentHealth = maxHealth;
    }
    
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"{enemyType} took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    public void ApplyDamage(float damage)
    {
        TakeDamage(damage);
    }
    
    void Die()
    {
        Debug.Log($"{enemyType} has died!");
        
        // Notify GameManager about enemy death
        OnEnemyDeath?.Invoke(enemyType);
        
        // Destroy the enemy
        Destroy(gameObject);
    }
}