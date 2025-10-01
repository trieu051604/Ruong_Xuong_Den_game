using UnityEngine;

public class Skeleton : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    // Map boundaries
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -5f;
    public float maxY = 5f;

    private Vector2 movement;
    private float changeDirectionTime = 2f; // how often to change direction or idle
    private float timer;

    [Header("Animation")]
    public Animator animator;             // Reference to Animator (to control animations)
    public SpriteRenderer spriteRenderer; // Reference to SpriteRenderer (to flip sprite)

    void Start()
    {
        timer = changeDirectionTime;
        PickRandomState();
    }

    void Update()
    {
        // Countdown timer for changing movement state
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            PickRandomState();              // Choose a new random state
            timer = changeDirectionTime;    // Reset timer
        }

        // Update animator parameters
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Flip sprite when moving left
        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }
    }

    void FixedUpdate()
    {
        // Calculate new position based on movement
        Vector2 newPos = rb.position + movement * speed * Time.fixedDeltaTime;

        // Boundary check on X-axis
        if (newPos.x < minX || newPos.x > maxX)
        {
            movement.x = -movement.x; // Reverse X direction if hitting boundary
            newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        }

        // Boundary check on Y-axis
        if (newPos.y < minY || newPos.y > maxY)
        {
            movement.y = -movement.y; // Reverse Y direction if hitting boundary
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);
        }

        // Move Rigidbody to the new position
        rb.MovePosition(newPos);
    }

    void PickRandomState()
    {
        int rand = Random.Range(0, 5);
        // 0 = idle, 1 = up, 2 = right, 3 = down, 4 = left

        switch (rand)
        {
            case 0:
                movement = Vector2.zero;   // Idle
                break;
            case 1:
                movement = Vector2.up;     // Move up
                break;
            case 2:
                movement = Vector2.right;  // Move right
                break;
            case 3:
                movement = Vector2.down;   // Move down
                break;
            case 4:
                movement = Vector2.left;   // Move left
                break;
        }
    }
}
