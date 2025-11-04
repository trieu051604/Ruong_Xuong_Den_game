using UnityEngine;

public class Slime : Monster
{
    [SerializeField] private GameObject energryObject;
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

    protected override void Die()
    {
        if (energryObject != null)
        {
            GameObject energry = Instantiate(energryObject, transform.position, Quaternion.identity);
            Destroy(energry, 5f);
        }
        base.Die();
    }
}
