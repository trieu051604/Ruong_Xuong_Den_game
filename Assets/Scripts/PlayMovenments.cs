using UnityEngine;

public class PlayMovenments : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    public Animator animator;
  
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;
    private bool isAttacking = false;


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

        if (Input.GetKeyDown(KeyCode.J))
        {
            StartAttack();
        }

    }


    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void StartAttack()
    {
        isAttacking = true;
        animator.SetBool("IsAttacking", true);
    }

    // Hàm này gọi từ Animation Event cuối clip tấn công
    public void EndAttack()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }
}
