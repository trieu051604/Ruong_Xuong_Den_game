using UnityEngine;
using System.Collections;

public class Skeleton_Swordman : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Movement Settings")]
    public float speed = 3f;
    public float detectRange = 1.5f;

    [Header("Attack Settings")]
    public float attackCooldown = 1.5f;
    public int attackDamage = 10; // ?? Damage per hit
    private bool isAttacking;
    private float attackTimer;

    [Header("Player Detection")]
    public Transform player;

    private PlayerHealth playerHealth; // Reference to player's health

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        attackTimer = attackCooldown;

        // Automatically find PlayerHealth component if available
        if (player != null)
            playerHealth = player.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if (!isAttacking)
        {
            if (distanceToPlayer > detectRange)
            {
                // --- Chase player ---
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * speed;

                // Update animator movement parameters
                animator.SetFloat("Horizontal", direction.x);
                animator.SetFloat("Vertical", direction.y);
                animator.SetFloat("Speed", rb.velocity.sqrMagnitude);

                // Flip sprite depending on direction
                if (direction.x != 0)
                    spriteRenderer.flipX = direction.x < 0;
            }
            else
            {
                // --- Player within attack range ---
                rb.velocity = Vector2.zero;

                // Face the player
                Vector2 dir = (player.position - transform.position).normalized;
                if (dir.x < 0)
                    spriteRenderer.flipX = true;
                else if (dir.x > 0)
                    spriteRenderer.flipX = false;

                // Attack if cooldown finished
                if (attackTimer <= 0f)
                {
                    StartCoroutine(Attack(dir));
                    attackTimer = attackCooldown;
                }
            }
        }
        else
        {
            // Stop moving while attacking
            rb.velocity = Vector2.zero;
        }
    }

    IEnumerator Attack(Vector2 dir)
    {
        isAttacking = true;

        // Reset all attack animation bools
        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
        animator.SetBool("AttackRight", false);

        // Determine attack direction based on player's relative position
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // Horizontal attack
            animator.SetBool("AttackRight", true);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            // Vertical attack
            if (dir.y > 0)
                animator.SetBool("AttackUp", true);
            else
                animator.SetBool("AttackDown", true);
            spriteRenderer.flipX = false;
        }

        // Wait a bit to sync with animation hit frame
        yield return new WaitForSeconds(0.4f);

        // --- Apply damage if player still in range ---
        if (playerHealth != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= detectRange + 0.2f) // small tolerance
            {
                playerHealth.TakeDamage(attackDamage);
                Debug.Log($"Skeleton hit the player for {attackDamage} damage!");
            }
        }

        // Wait for the rest of the animation
        yield return new WaitForSeconds(0.4f);

        // Reset attack animation states
        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
        animator.SetBool("AttackRight", false);

        isAttacking = false;
    }
}
