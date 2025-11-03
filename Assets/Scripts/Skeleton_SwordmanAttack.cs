using UnityEngine;
using UnityEngine.SceneManagement;

public class Skeleton_SwordmanAttack : MonoBehaviour
{
    [Header("Movement & Detection")]
    public float moveSpeed = 2f;           // Skeleton movement speed
    public float attackRange = 1.2f;       // Distance to attack the player
    public float detectionRange = 10f;     // Distance to start chasing the player

    [Header("Attack Settings")]
    public int attackDamage = 20;          // Damage per hit
    public float attackRate = 1.5f;        // Attacks per second
    public Transform attackPoint;          // Origin point of attack (front of skeleton)
    public LayerMask playerLayer;          // Player layer for detection

    private Transform player;
    private PlayerHealth playerHealth;
    private float nextAttackTime = 0f;
    private bool isFacingRight = true;

    void Start()
    {
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerHealth = playerObj.GetComponent<PlayerHealth>();
        }
    }

    void Update()
    {
        // Stop all actions if player is dead
        if (player == null || playerHealth == null || !playerHealth.IsAlive())
            return;

        float distance = Vector2.Distance(transform.position, player.position);

        FlipTowardsPlayer();

        if (distance <= attackRange)
        {
            // Attack player if within attack range
            if (Time.time >= nextAttackTime)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        else if (distance <= detectionRange)
        {
            // Move towards player if within detection range but outside attack range
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    void Attack()
    {
        if (playerHealth == null || !playerHealth.IsAlive())
            return; // Do not attack if player is dead

        // Detect player in attack range
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D p in hitPlayers)
        {
            PlayerHealth health = p.GetComponent<PlayerHealth>();
            if (health != null && health.IsAlive())
            {
                health.TakeDamage(attackDamage);

                // Disable player's collider if player dies to stop further attacks
                if (!health.IsAlive())
                {
                    Collider2D playerCollider = p.GetComponent<Collider2D>();
                    if (playerCollider != null)
                        playerCollider.enabled = false;
                }
            }
        }
    }

    void FlipTowardsPlayer()
    {
        if (player == null) return;

        if (player.position.x > transform.position.x && !isFacingRight)
            Flip();
        else if (player.position.x < transform.position.x && isFacingRight)
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Draw gizmos to visualize attack & detection range
    private void OnDrawGizmosSelected()
    {
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
