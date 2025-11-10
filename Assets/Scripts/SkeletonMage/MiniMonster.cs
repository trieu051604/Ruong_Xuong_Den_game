using UnityEngine;

public class MiniMonster : Monster
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamge(enterDamage);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.TakeDamge(stayDamage);
        }
    }


}
