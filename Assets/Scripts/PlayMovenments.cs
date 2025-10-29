using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PlayMovenments : MonoBehaviour
{
    public static PlayMovenments Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float speed = 5f;
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    private bool isAttacking = false;

    [Header("Attack Settings")]
    public float attackRange = 1f;
    public LayerMask enemyLayers;  // ⚠️ chọn layer Enemy trong Inspector
    public int attackDamage = 10;

    private Vector2 attackDirection = Vector2.down; // hướng tấn công cuối cùng

    void Update()
    {
        // Di chuyển
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
            attackDirection = movement.normalized;

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;

        // Tấn công
        if (Input.GetKeyDown(KeyCode.J) && !isAttacking)
            StartAttack();
    }

    void FixedUpdate()
    {
        if (!isAttacking)
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);

        // Cập nhật hướng đánh
        animator.SetFloat("AttackX", attackDirection.x);
        animator.SetFloat("AttackY", attackDirection.y);

        // Tính vị trí đánh (ở trước mặt nhân vật)
        Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange * 0.5f;

        // ✅ Kiểm tra enemy trong phạm vi
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPos, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Đã đánh trúng: " + enemy.name);
            enemy.GetComponent<Skeleton_Bowman>()?.TakeDamage(attackDamage);
        }

        // Kết thúc sau 0.3s
        Invoke(nameof(EndAttack), 0.3f);
    }

    void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    // Vẽ phạm vi tấn công
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackPos = (Vector2)transform.position + attackDirection * attackRange * 0.5f;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}
