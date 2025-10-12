using UnityEngine;

public class Bombschroom_Move : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    // Map boundaries
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("Health Settings")]
    public float maxHealth = 75f;
    public float currentHealth;
    public EnemyHealth.EnemyType enemyType = EnemyHealth.EnemyType.Bombschroom;

    [Header("AI Settings")]
    public float detectionRange = 3f;
    public float attackRange = 1f;
    public float attackDamage = 20f;
    public float attackCooldown = 2f;

    private Vector2 movement;
    private float changeDirectionTime = 2f;
    private float timer;
    private float lastAttackTime = -999f;
    private bool isAttacking = false;

    private Transform player;
    private enum State { Wander, Chase, Attack }
    private State currentState = State.Wander;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        timer = changeDirectionTime;
        PickRandomState();

        // Initialize health
        currentHealth = maxHealth;

        // Find player
        if (PlayMovenments.Instance != null)
        {
            player = PlayMovenments.Instance.transform;
        }

        // Ensure Rigidbody2D settings
        if (rb != null)
        {
            rb.gravityScale = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    }

    void Update()
    {
        if (player == null)
        {
            Wander();
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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
                if (!isAttacking)
                {
                    Chase();
                }

                if (distanceToPlayer <= attackRange && !isAttacking)
                {
                    currentState = State.Attack;
                }
                else if (distanceToPlayer > detectionRange * 1.2f)
                {
                    currentState = State.Wander;
                }
                break;

            case State.Attack:
                Attack();

                if (!isAttacking && distanceToPlayer > attackRange * 1.1f)
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
        if (isAttacking) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomState();
            timer = changeDirectionTime;
        }

        UpdateAnimation();
        MoveWithBoundaries();
    }

    void Chase()
    {
        if (isAttacking || player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        movement = direction;
        UpdateAnimation();
        MoveWithBoundaries();
    }

    void Attack()
    {
        if (isAttacking || player == null) return;

        // Stop movement during attack
        movement = Vector2.zero;
        UpdateAnimation();

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        // Play attack animation (if you have one)
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Deal damage to player if in range
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= attackRange)
        {
            // Try to damage player
            var playerHealth = player.GetComponent<MonoBehaviour>();
            if (playerHealth != null)
            {
                player.SendMessage("TakeDamage", attackDamage, SendMessageOptions.DontRequireReceiver);
                Debug.Log($"Bombschroom attacked player for {attackDamage} damage!");
            }
        }

        // End attack after a short duration
        Invoke(nameof(EndAttack), 0.8f);
    }

    void EndAttack()
    {
        isAttacking = false;
    }

    void MoveWithBoundaries()
    {
        if (rb == null) return;

        // Calculate new position based on movement
        Vector2 newPos = rb.position + movement * speed * Time.fixedDeltaTime;

        // Boundary check on X-axis
        if (newPos.x < minX || newPos.x > maxX)
        {
            movement.x = -movement.x; // Reverse X direction if hitting boundary
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        }

        // Boundary check on Y-axis
        if (newPos.y < minY || newPos.y > maxY)
        {
            movement.y = -movement.y; // Reverse Y direction if hitting boundary
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        }

        // Move Rigidbody to the new position
        rb.linearVelocity = Vector2.zero; // Prevent conflicts
        rb.MovePosition(newPos);
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        // Update animator parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip sprite when moving left
        if (movement.x != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    void PickRandomState()
    {
        int rand = Random.Range(0, 5);
        // 0 = idle, 1 = up, 2 = right, 3 = down, 4 = left

        switch (rand)
        {
            case 0:
                movement = Vector2.zero;   // Idle
                break;
            case 1:
                movement = Vector2.up;     // Move up
                break;
            case 2:
                movement = Vector2.right;  // Move right
                break;
            case 3:
                movement = Vector2.down;   // Move down
                break;
            case 4:
                movement = Vector2.left;   // Move left
                break;
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
    }

    /// <summary>
    /// Method called by Player attack system
    /// </summary>
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log($"Bombschroom took {damage} damage. Health: {currentHealth}/{maxHealth}");

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
        Debug.Log($"Bombschroom has died!");

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
