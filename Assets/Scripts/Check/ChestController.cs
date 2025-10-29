using UnityEngine;

public class ChestController : MonoBehaviour
{
    public Animator animator;
    private bool openedOnce = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Lần đầu mở
        if (!openedOnce && collision.CompareTag("Player"))
        {
            openedOnce = true;
            animator.SetBool("IsOpen", true);
            animator.SetBool("Opened", true);  
        }
        // Những lần sau
        else if (openedOnce && collision.CompareTag("Player"))
        {
            animator.SetBool("IsOpen", false); 
        }
    }
}
