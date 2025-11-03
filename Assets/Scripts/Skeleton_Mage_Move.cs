using UnityEngine;

public class Skeleton_Mage_Move : MonoBehaviour
{
    // Core Components & Stats 
    public Rigidbody2D rb;
    public float speed = 3f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    // AI Behavior Variables 
    public float detectionRange = 8f;   // Range to detect the player
    public float attackRange = 1.5f;    // Range to attack the player
    public float attackCooldown = 2f;   // Cooldown between attacks

    private Transform playerTransform;      // Reference to the player's transform
    private float lastAttackTime = -999f;   // The time of the last attack

    //  Wander State Variables 
    private Vector2 wanderMovement;
    private float changeDirectionTime = 2f;
    private float timer;

    // State Machine for AI
    private enum State { Wander, Chase, Attack }
    private State currentState;

    void Start()
    {
        
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        timer = changeDirectionTime;
        currentState = State.Wander; 
    }

    void Update()
    {
        if (playerTransform == null)
        {
            
            currentState = State.Wander;
            Wander();
            return;
        }

        
        switch (currentState)
        {
            case State.Wander:
                Wander();
                
                if (Vector2.Distance(transform.position, playerTransform.position) < detectionRange)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                Chase();
                
                if (Vector2.Distance(transform.position, playerTransform.position) < attackRange)
                {
                    currentState = State.Attack;
                }
                
                else if (Vector2.Distance(transform.position, playerTransform.position) > detectionRange)
                {
                    currentState = State.Wander;
                }
                break;

            case State.Attack:
                Attack();
                
                if (Vector2.Distance(transform.position, playerTransform.position) > attackRange)
                {
                    currentState = State.Chase;
                }
                break;
        }
    }

    void Wander()
    {
        // Random wandering logic
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomDirection();
            timer = changeDirectionTime;
        }
        UpdateAnimation(wanderMovement);
        rb.MovePosition(rb.position + wanderMovement * speed * Time.fixedDeltaTime);
    }

    void Chase()
    {
        // Move towards the player
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        UpdateAnimation(direction);
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        if (playerTransform == null) return;

     
        Vector2 directionToPlayer = (playerTransform.position - transform.position);
        if (Mathf.Abs(directionToPlayer.x) > Mathf.Abs(directionToPlayer.y))
        {
            spriteRenderer.flipX = directionToPlayer.x < 0;
        }

       
        UpdateAnimation(Vector2.zero);

       
        if (Time.time >= lastAttackTime + attackCooldown)
        {
           
            rb.linearVelocity = Vector2.zero;

            lastAttackTime = Time.time;

            
            directionToPlayer.Normalize();
            animator.SetFloat("AttackX", directionToPlayer.x);
            animator.SetFloat("AttackY", directionToPlayer.y);
            animator.SetTrigger("Attack");
        }
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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}