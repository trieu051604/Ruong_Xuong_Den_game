using UnityEngine;
using System.Collections;

public class Skeleton_Swordman : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;

    [Header("Movement Boundaries")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    private Vector2 movement;
    private float changeDirectionTime = 2f;
    private float timer;

    [Header("Animation")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Attack Settings")]
    public float detectRange = 1.5f; // Ph?m vi ph�t hi?n player
    public float attackCooldown = 1.5f;
    private bool isAttacking;
    private float attackTimer;

    [Header("Player Detection")]
    public Transform player; // K�o player v�o ?�y trong Inspector

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        timer = changeDirectionTime;
        attackTimer = attackCooldown;

        PickRandomState();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        attackTimer -= Time.deltaTime;

        if (!isAttacking)
        {
            if (distanceToPlayer <= detectRange)
            {
                // --- Khi player trong ph?m vi t?n c�ng ---
                rb.linearVelocity = Vector2.zero; // D?NG di chuy?n ho�n to�n
                movement = Vector2.zero;

                // H??ng v? ph�a player
                Vector2 dir = (player.position - transform.position).normalized;

                // L?t h??ng sprite (n?u d�ng flipX thay v� animation tr�i/ph?i)
                if (dir.x < 0)
                    spriteRenderer.flipX = true;
                else if (dir.x > 0)
                    spriteRenderer.flipX = false;

                // N?u t?i l�c t?n c�ng th� t?n c�ng
                if (attackTimer <= 0f)
                {
                    StartCoroutine(Attack(dir));
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                // --- Player ? xa: di chuy?n ng?u nhi�n ---
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    PickRandomState();
                    timer = changeDirectionTime;
                }

                // Cho nh�n v?t di chuy?n b�nh th??ng khi kh�ng t?n c�ng
                rb.linearVelocity = movement * speed;
            }
        }
        else
        {
            // ?ang t?n c�ng th� KH�NG di chuy?n
            rb.linearVelocity = Vector2.zero;
        }

        // ? Ph?n animator ph?i n?m b�n trong Update()
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // L?t sprite theo h??ng di chuy?n (ch? khi kh�ng t?n c�ng)
        if (!isAttacking && movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;
    }

    void FixedUpdate()
    {
        if (isAttacking) return;

        Vector2 newPos = rb.position + movement * speed * Time.fixedDeltaTime;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        rb.MovePosition(newPos);
    }

    void PickRandomState()
    {
        int rand = Random.Range(0, 5);
        switch (rand)
        {
            case 0: movement = Vector2.zero; break;
            case 1: movement = Vector2.up; break;
            case 2: movement = Vector2.right; break;
            case 3: movement = Vector2.down; break;
            case 4: movement = Vector2.left; break;
        }
    }

    IEnumerator Attack(Vector2 dir)
    {
        isAttacking = true;

        // Reset t?t c? bool Attack
        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
        animator.SetBool("AttackRight", false);

        // X�c ??nh h??ng ?�nh
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // ?�nh ngang
            animator.SetBool("AttackRight", true);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            // ?�nh d?c
            if (dir.y > 0)
                animator.SetBool("AttackUp", true);
            else
                animator.SetBool("AttackDown", true);
            spriteRenderer.flipX = false;
        }

        yield return new WaitForSeconds(0.8f); // th?i gian animation ?�nh

        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
        animator.SetBool("AttackRight", false);

        isAttacking = false;
    }
}
