using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] private GameObject usbObject;
    [SerializeField] private AudioManagementLevel1 audioManagementLevel1;
    private float attackSoundCooldown = 0.5f;
    private float lastAttackTime;
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

                if (Time.time - lastAttackTime > attackSoundCooldown)
                {
                    audioManagementLevel1.PlayBossAttackSoundLevel1();
                    lastAttackTime = Time.time;
                }
            }
        }
    }

    protected override void Die()
    {
        if (usbObject != null)
        {
            Instantiate(usbObject, transform.position, Quaternion.identity);
        }
        base.Die();
    }
}
