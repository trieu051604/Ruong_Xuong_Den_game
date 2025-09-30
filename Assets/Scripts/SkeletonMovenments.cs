using UnityEngine;

public class Skeleton_Move : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 2f;

    // Giới hạn bản đồ
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    private Vector2 movement;
    private float changeDirectionTime = 2f;
    private float timer;

    [Header("Animation")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null) animator = GetComponent<Animator>();
    }

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
        if (movement == Vector2.zero) return;

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

        if (rb != null)
        {
            Vector2 pos = rb.position;

            if (pos.x <= minX) movement = Vector2.right;
            if (pos.x >= maxX) movement = Vector2.left;
            if (pos.y <= minY) movement = Vector2.up;
            if (pos.y >= maxY) movement = Vector2.down;
        }

        Debug.Log("Skeleton new state: " + movement);
    }
}
