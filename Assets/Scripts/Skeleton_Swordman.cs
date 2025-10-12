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
    public float detectRange = 1.5f; // Ph?m vi phát hi?n player
    public float attackCooldown = 1.5f;
    private bool isAttacking;
    private float attackTimer;

    [Header("Player Detection")]
    public Transform player; // Kéo player vào ?ây trong Inspector

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
                // --- Khi player trong ph?m vi t?n công ---
                rb.velocity = Vector2.zero; // D?NG di chuy?n hoàn toàn
                movement = Vector2.zero;

                // H??ng v? phía player
                Vector2 dir = (player.position - transform.position).normalized;

                // L?t h??ng sprite (n?u dùng flipX thay vì animation trái/ph?i)
                if (dir.x < 0)
                    spriteRenderer.flipX = true;
                else if (dir.x > 0)
                    spriteRenderer.flipX = false;

                // N?u t?i lúc t?n công thì t?n công
                if (attackTimer <= 0f)
                {
                    StartCoroutine(Attack(dir));
                    attackTimer = attackCooldown;
                }
            }
            else
            {
                // --- Player ? xa: di chuy?n ng?u nhiên ---
                timer -= Time.deltaTime;
                if (timer <= 0f)
                {
                    PickRandomState();
                    timer = changeDirectionTime;
                }

                // Cho nhân v?t di chuy?n bình th??ng khi không t?n công
                rb.velocity = movement * speed;
            }
        }
        else
        {
            // ?ang t?n công thì KHÔNG di chuy?n
            rb.velocity = Vector2.zero;
        }

        // ? Ph?n animator ph?i n?m bên trong Update()
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // L?t sprite theo h??ng di chuy?n (ch? khi không t?n công)
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

        // Xác ??nh h??ng ?ánh
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // ?ánh ngang
            animator.SetBool("AttackRight", true);
            spriteRenderer.flipX = dir.x < 0;
        }
        else
        {
            // ?ánh d?c
            if (dir.y > 0)
                animator.SetBool("AttackUp", true);
            else
                animator.SetBool("AttackDown", true);
            spriteRenderer.flipX = false;
        }

        yield return new WaitForSeconds(0.8f); // th?i gian animation ?ánh

        animator.SetBool("AttackUp", false);
        animator.SetBool("AttackDown", false);
        animator.SetBool("AttackRight", false);

        isAttacking = false;
    }
}
