using UnityEngine;
using UnityEngine.Events;

public class Skeleton_Bowman : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;
    public HealthBar healthBar;
    public UnityEvent OnDeath;

    [Header("Movement Settings")]
    public Rigidbody2D rb;
    public float speed = 3f;
    public float minX = -5f, maxX = 5f, minY = -5f, maxY = 5f;

    private Vector2 movement;
    private float changeDirectionTime = 2f;
    private float timer;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector2 lastDirection; // hướng di chuyển cuối cùng
    private bool isDead = false;

    private void OnEnable()
    {
        OnDeath.AddListener(Death);
    }

    private void OnDisable()
    {
        OnDeath.RemoveListener(Death);
    }

    void Start()
    {
        timer = changeDirectionTime;
        currentHealth = maxHealth;
        healthBar.UpdateBar(currentHealth, maxHealth);
        PickRandomState();
    }

    void Update()
    {
        if (isDead) return;

        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomState();
            timer = changeDirectionTime;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
            spriteRenderer.flipX = movement.x < 0;

        // Test manual damage (tạm để debug)
        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(20);
        }
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Vector2 newPos = rb.position + movement * speed * Time.fixedDeltaTime;

        if (newPos.x < minX || newPos.x > maxX)
        {
            movement.x = -movement.x;
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        }
        if (newPos.y < minY || newPos.y > maxY)
        {
            movement.y = -movement.y;
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        }

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
        lastDirection = movement;
    }

    // ✅ Khi bị Player đánh gần
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth < 0)
            currentHealth = 0;

        healthBar.UpdateBar(currentHealth, maxHealth);
        StartCoroutine(HitFlash());

        if (currentHealth <= 0)
        {
            isDead = true;
            OnDeath.Invoke();
        }
    }

    public void Death()
    {
        animator.SetTrigger("Death");

        rb.linearVelocity = Vector2.zero;  // Dừng di chuyển
        rb.isKinematic = true;       // Ngắt tương tác vật lý
        GetComponent<Collider2D>().enabled = false; // Tắt collider

        Destroy(gameObject, 1.5f); // Xóa sau khi animation chạy xong
    }



    private System.Collections.IEnumerator HitFlash()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    // ✅ Khi va chạm Player → quái đi ngược lại
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            movement = -movement;
        }
    }
}
