using UnityEngine;

public class Skeleton_Bowman : MonoBehaviour
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

        // Prevent going outside boundaries
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rb.MovePosition(newPos);
    }

    void PickRandomState()
    {
        int rand = Random.Range(0, 5);

        switch (rand)
        {
            case 0:
                movement = Vector2.zero;
                break;
            case 1:
                movement = Vector2.up;
                break;
            case 2:
                movement = Vector2.right;
                break;
            case 3:
                movement = Vector2.down;
                break;
            case 4:
                movement = Vector2.left;
                break;
        }

        Debug.Log("Random Movement: " + movement);
    }
}