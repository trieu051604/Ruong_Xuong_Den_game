using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] private GameObject usbObject;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamge(enterDamage);
            }
        }
    }

    protected override void Die()
    { 
        Instantiate(usbObject, transform.position, Quaternion.identity);
        base.Die();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamge(stayDamage);
            }
        }
    }
}

