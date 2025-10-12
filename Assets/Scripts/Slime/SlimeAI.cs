using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 50f;
    public float currentHealth;
    public EnemyHealth.EnemyType enemyType = EnemyHealth.EnemyType.Slime;
    
    [Header("AI Settings")]
    [SerializeField] private float roamChangeDirFloat = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private MonoBehaviour enemyType_Old; // Keep old reference for compatibility
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private bool stopMovingWhileAttacking = false;

    private bool canAttack = true;

    private enum State
    {
        Roaming,
        Attacking
    }

    private Vector2 roamPosition;
    private float timeRoaming = 0f;

    private State state;
    private EnemyPathfinding enemyPathfinding;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start()
    {
        roamPosition = GetRoamingPosition();
        // Initialize health
        currentHealth = maxHealth;
    }

    private void Update()
    {
        MovementStateControl();
    }

    private void MovementStateControl()
    {
        switch (state)
        {
            default:
            case State.Roaming:
                Roaming();
                break;

            case State.Attacking:
                Attacking();
                break;
        }
    }

    private void Roaming()
    {
        timeRoaming += Time.deltaTime;

        if (enemyPathfinding != null)
        {
            enemyPathfinding.MoveTo(roamPosition);
        }

        if (PlayMovenments.Instance != null && Vector2.Distance(transform.position, PlayMovenments.Instance.transform.position) < attackRange)
        {
            state = State.Attacking;
        }

        if (timeRoaming > roamChangeDirFloat)
        {
            roamPosition = GetRoamingPosition();
        }
    }

    private void Attacking()
    {
        if (PlayMovenments.Instance == null || Vector2.Distance(transform.position, PlayMovenments.Instance.transform.position) > attackRange)
        {
            state = State.Roaming;
            return; 
        }

        if (attackRange != 0 && canAttack)
        {
            canAttack = false;
            
            // Use old enemyType reference if available
            if (enemyType_Old != null)
            {
                (enemyType_Old as IEnemy)?.Attack();
            }

            if (stopMovingWhileAttacking)
            {
                enemyPathfinding.StopMoving();
            }
            else
            {
                enemyPathfinding.MoveTo(roamPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine()
    {
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private Vector2 GetRoamingPosition()
    {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    /// <summary>
    /// Method called by Player attack system
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Slime took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// Alternative method name for compatibility
    /// </summary>
    public void ApplyDamage(float damage)
    {
        TakeDamage(damage);
    }

    void Die()
    {
        Debug.Log($"Slime has died!");
        
        // Notify LevelManager about enemy death
        LevelManager levelManager = Object.FindFirstObjectByType<LevelManager>();
        if (levelManager != null)
        {
            levelManager.OnEnemyKilled(enemyType);
        }
        
        // Destroy the enemy
        Destroy(gameObject);
    }
}
