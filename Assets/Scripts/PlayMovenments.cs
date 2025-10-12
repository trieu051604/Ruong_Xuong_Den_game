using UnityEngine;

public class PlayMovenments : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    public Animator animator;
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;
    private bool isAttacking = false;

    // phạm vi tấn công
    public float attackRange = 1f;
    public LayerMask enemyLayers;

    // hướng tấn công cuối cùng
    private Vector2 attackDirection = Vector2.down; // mặc định xuống

    public static PlayMovenments Instance { get; private set; }

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            attackDirection = movement.normalized; // lưu hướng cuối cùng khi có di chuyển
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }

        // Nhấn J để tấn công
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
        {
            StartAttack();
        }
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);

        // Chọn animation attack theo hướng
        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);

        // vị trí trung tâm của cú đánh
        Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange * 0.5f;

        // Detect enemies in attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);
        foreach (Collider2D hit in hitEnemies)
        {
            Debug.Log("Đã đánh trúng: " + hit.name);

            bool damageDealt = false;

            // Try to find EnemyHealth on the collider or its parents
            EnemyHealth enemyHealth = hit.GetComponentInParent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(50f);
                damageDealt = true;
                Debug.Log($"Player dealt 50 damage to {enemyHealth.enemyType} ({enemyHealth.gameObject.name}) via EnemyHealth");
            }

            // If still not dealt, try direct component on collider
            if (!damageDealt)
            {
                enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(50f);
                    damageDealt = true;
                    Debug.Log($"Player dealt 50 damage to {enemyHealth.enemyType} ({enemyHealth.gameObject.name}) via EnemyHealth (on collider)");
                }
            }

            // Fallback: send message to collider and to parent
            if (!damageDealt)
            {
                hit.SendMessage("TakeDamage", 50f, SendMessageOptions.DontRequireReceiver);
                hit.SendMessage("ApplyDamage", 50f, SendMessageOptions.DontRequireReceiver);
                if (hit.transform.parent != null)
                {
                    hit.transform.parent.SendMessage("TakeDamage", 50f, SendMessageOptions.DontRequireReceiver);
                    hit.transform.parent.SendMessage("ApplyDamage", 50f, SendMessageOptions.DontRequireReceiver);
                }

                Debug.Log($"Player dealt 50 damage to {hit.name} via SendMessage fallback");
            }
        }

        Invoke(nameof(EndAttack), 0.3f);
    }

    void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange * 0.5f;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
