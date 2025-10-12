using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 200f;
    public float currentHealth;
    public EnemyHealth.EnemyType enemyType = EnemyHealth.EnemyType.Skeleton;
    // Core Components & Stats 
    public Rigidbody2D rb;
    public float speed = 3f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // AI Behavior Variables 
    public float detectionRange = 8f;   // Range to detect the player
    public float attackRange = 1.5f;    // Range to attack the player
    public float attackCooldown = 2f;   // Cooldown between attacks
    public float attackDamage = 10f;    // Damage dealt to player
    public LayerMask playerLayer = 1;   // Layer mask for player detection

    private Transform playerTransform;      // Reference to the player's transform
    private float lastAttackTime = -999f;   // The time of the last attack
    private bool isAttacking = false;       // Is currently attacking

    //  Wander State Variables 
    private Vector2 wanderMovement;
    private float changeDirectionTime = 2f;
    private float timer;

    // current movement applied in FixedUpdate
    private Vector2 currentMovement;

    // State Machine for AI
    private enum State { Wander, Chase, Attack }
    private State currentState;

    void Start()
    {
        // Initialize health
        currentHealth = maxHealth;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        timer = changeDirectionTime;
        currentState = State.Wander;

        // Safety: ensure Rigidbody2D physics won't rotate or be affected by gravity
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.angularVelocity = 0f;
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (playerTransform == null)
        {
            currentState = State.Wander;
            Wander();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case State.Wander:
                Wander();

                if (distanceToPlayer < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                // Chỉ chase khi không đang tấn công
                if (!isAttacking)
                {
                    Chase();
                }

                if (distanceToPlayer <= attackRange && !isAttacking)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange * 1.2f) // Thêm hysteresis để tránh flickering
                {
                    currentState = State.Wander;
                }
                break;

            case State.Attack:
                Attack();

                // Chỉ chuyển state khi không đang tấn công và player đã ra khỏi vùng tấn công
                if (!isAttacking && distanceToPlayer > attackRange * 1.1f) // Thêm hysteresis
                {
                    if (distanceToPlayer <= detectionRange)
                    {
                        currentState = State.Chase;
                    }
                    else
                    {
                        currentState = State.Wander;
                    }
                }
                break;
        }
    }

    void Wander()
    {
        if (isAttacking)
        {
            currentMovement = Vector2.zero;
            return; // Không wander khi đang tấn công
        }

        // Random wandering logic
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomDirection();
            timer = changeDirectionTime;
        }

        UpdateAnimation(wanderMovement);

        // set movement; actual MovePosition happens in FixedUpdate
        currentMovement = wanderMovement;
    }

    void Chase()
    {
        if (isAttacking)
        {
            currentMovement = Vector2.zero;
            return; // Không chase khi đang tấn công
        }

        // Move towards the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        UpdateAnimation(direction);

        // set movement; actual MovePosition happens in FixedUpdate
        currentMovement = direction;
    }

    void Attack()
    {
        if (playerTransform == null || isAttacking) return;

        // KHÔNG quay mặt về phía player nữa để tránh xoay vòng vòng

        // Dừng animation di chuyển
        animator.SetFloat("Horizontal", 0);
        animator.SetFloat("Vertical", 0);
        animator.SetFloat("Speed", 0);

        // Tấn công nếu đã hết cooldown
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            StartAttack(directionToPlayer);
        }

        // stop movement while attacking
        currentMovement = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Apply movement via MovePosition in FixedUpdate to avoid conflicts
        if (rb == null) return;

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Ensure no unexpected velocity
        rb.linearVelocity = Vector2.zero;

        Vector2 newPos = rb.position + currentMovement * speed * Time.fixedDeltaTime;
        rb.MovePosition(newPos);
    }

    void StartAttack(Vector2 attackDirection)
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Stop all movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Chỉ dùng Attack_Up animation đơn giản
        animator.SetBool("Attack_Up", true);

        // Perform actual attack damage detection
        PerformAttackDamage(attackDirection);

        // End attack after animation time
        Invoke(nameof(EndAttack), 0.6f);
    }

    void PerformAttackDamage(Vector2 attackDirection)
    {
        // Calculate attack position
        Vector2 attackPosition = (Vector2)transform.position + attackDirection * (attackRange * 0.7f);

        // Detect all colliders in attack range
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(attackPosition, attackRange * 0.8f, playerLayer);

        foreach (Collider2D hit in hitColliders)
        {
            // Check if it's the player
            if (hit.CompareTag("Player"))
            {
                Debug.Log($"Skeleton attacked {hit.name} for {attackDamage} damage!");

                // Try to damage player using different possible methods
                hit.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
                hit.SendMessage("ApplyDamage", attackDamage, SendMessageOptions.DontRequireReceiver);

                // Visual feedback
                Debug.Log("Player hit by Skeleton attack!");
            }
        }
    }

    void EndAttack()
    {
        isAttacking = false;

        // Tắt animation Attack_Up
        animator.SetBool("Attack_Up", false);
    }

    void PickRandomDirection()
    {
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: wanderMovement = Vector2.zero; break;
            case 1: wanderMovement = Vector2.up; break;
            case 2: wanderMovement = Vector2.right; break;
            case 3: wanderMovement = Vector2.down; break;
            case 4: wanderMovement = Vector2.left; break;
        }
    }

    void UpdateAnimation(Vector2 movement)
    {
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip sprite based on movement direction (for Wander and Chase states)
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }


    void OnDrawGizmosSelected()
    {
        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        // Attack damage area when attacking
        if (isAttacking && playerTransform != null)
        {
            Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Vector2 attackPos = (Vector2)transform.position + directionToPlayer * (attackRange * 0.7f);

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(attackPos, attackRange * 0.8f);
        }
    }
    /// <summary>
    /// Method called by Player attack system
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Skeleton took {damage} damage. Health: {currentHealth}/{maxHealth}");

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
        Debug.Log($"Skeleton has died!");

        // Notify LevelManager about enemy death
        LevelManager levelManager = GetLevelManager();
        if (levelManager != null)
        {
            levelManager.OnEnemyKilled(enemyType);
        }

        // Destroy the enemy
        Destroy(gameObject);
    }

    private LevelManager GetLevelManager()
    {
        // Use the newer API when available to avoid the obsolete warning.
#if UNITY_2023_2_OR_NEWER
        return FindFirstObjectByType<LevelManager>();
#else
            return FindObjectOfType<LevelManager>();
#endif
    }
}
