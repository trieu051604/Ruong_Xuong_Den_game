using UnityEngine;

public class Skeleton : Monster
{
    [SerializeField] private GameObject usbObject;
    [SerializeField] private AudioManagementLevel1 audioManagementLevel1;
    public int enemyLevel = 3; 
    public GameObject winScreen; 


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
                audioManagementLevel1.PlayBossAttackSoundLevel1();
            }  
            }
    }

    protected override void Die()
    {
        if (usbObject != null)
        {
            Instantiate(usbObject, transform.position, Quaternion.identity);
        }
        
        if (enemyLevel == 3 && winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f; 
        }
        base.Die();
    }
}
