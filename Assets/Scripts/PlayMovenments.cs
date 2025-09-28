using UnityEngine;

public class PlayMovenments : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    public Animator animator;
  
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;


    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);


        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }



    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
