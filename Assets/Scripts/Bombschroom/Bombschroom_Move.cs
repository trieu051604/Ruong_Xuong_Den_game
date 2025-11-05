using UnityEngine;

public class Bombschroom_Move : Monster
{
    [SerializeField] private GameObject energryObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.TakeDamge(enterDamage);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
