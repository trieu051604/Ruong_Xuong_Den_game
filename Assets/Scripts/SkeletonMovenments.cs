using UnityEngine;

public class SteletonController : MonoBehaviour
{
    public Rigidbody2D steleton
        ;
    public float speed = 5f;

    public Animator animator;

    private Vector2 movement;


    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

    }
    private void FixedUpdate()
    {
        steleton.MovePosition(steleton.position + movement * speed * Time.fixedDeltaTime);
    }
}
