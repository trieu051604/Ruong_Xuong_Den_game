using UnityEngine;

public class Skeleton_Bowman : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 3f;

    public float minX = -5f, maxX = 5f, minY = -5f, maxY = 5f;

    private Vector2 movement;
    private float changeDirectionTime = 2f;
    private float timer;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private Vector2 lastDirection; // hướng di chuyển trước khi va chạm

    void Start()
    {
        timer = changeDirectionTime;
        PickRandomState();
    }

    void Update()
    {
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
    }

    void FixedUpdate()
    {
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

    // ✅ Khi va chạm Player → quái đi ngược lại
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            movement = -movement;
        }
    }
}
