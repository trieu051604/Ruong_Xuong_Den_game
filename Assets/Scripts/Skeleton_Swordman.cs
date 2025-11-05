using UnityEngine;
using System.Collections;

public class Skeleton_Swordman : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 3f;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Movement Boundaries")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    [Header("AI Behavior")]
    public float detectionRange = 5f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1.5f;

    private Transform playerTransform;
    private float lastAttackTime = -999f;

    private Vector2 wanderMovement;
    private float changeDirectionTime = 2f;
    private float timer;

    private enum State { Wander, Chase, Attack }
    private State currentState;
    private bool isAttacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;

        timer = changeDirectionTime;
        currentState = State.Wander;
    }

    void Update()
    {
        if (playerTransform == null)
        {
            currentState = State.Wander;
            animator.SetFloat("Horizontal", wanderMovement.x);
            animator.SetFloat("Vertical", wanderMovement.y);
            animator.SetFloat("Speed", wanderMovement.sqrMagnitude);
            return;
        }

        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;

            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        switch (currentState)
        {
            case State.Wander:
                if (distanceToPlayer < detectionRange) currentState = State.Chase;
                break;

            case State.Chase:
                if (distanceToPlayer < attackRange) currentState = State.Attack;
                else if (distanceToPlayer > detectionRange) currentState = State.Wander;
                break;

            case State.Attack:
                if (distanceToPlayer > attackRange) currentState = State.Chase;
                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    lastAttackTime = Time.time;
                    StartCoroutine(AttackCoroutine());
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (isAttacking || rb == null) return;

        Vector2 direction;

        if (currentState == State.Wander)
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0f)
            {
                PickRandomDirection();
                timer = changeDirectionTime;
            }
            direction = wanderMovement;
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);
            if (direction.x != 0) spriteRenderer.flipX = direction.x < 0;
        }
        else if (currentState == State.Chase && playerTransform != null)
        {
            direction = (playerTransform.position - transform.position).normalized;
            animator.SetFloat("Horizontal", direction.x);
            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Speed", direction.sqrMagnitude);
            if (direction.x != 0) spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            direction = Vector2.zero;
            animator.SetFloat("Speed", 0);
        }

        Vector2 newPos = rb.position + direction * speed * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
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

    IEnumerator AttackCoroutine()
    {
        isAttacking = true;
        Vector2 dir = (playerTransform.position - transform.position).normalized;

        animator.SetFloat("Speed", 0);
        animator.SetFloat("AttackX", dir.x);
        animator.SetFloat("AttackY", dir.y);
        animator.SetTrigger("Attack");

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            spriteRenderer.flipX = dir.x < 0;
        }

        yield return new WaitForSeconds(0.8f);

        isAttacking = false;
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(maxX, minY));
        Gizmos.DrawLine(new Vector2(minX, maxY), new Vector2(maxX, maxY));
        Gizmos.DrawLine(new Vector2(minX, minY), new Vector2(minX, maxY));
        Gizmos.DrawLine(new Vector2(maxX, minY), new Vector2(maxX, maxY));
    }
}
