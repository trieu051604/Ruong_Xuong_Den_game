using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected PlayMovenments player;
    [SerializeField] protected float maxHp = 50f;
    protected float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] protected float enterDamage = 10f;
    [SerializeField] protected float stayDamage = 1f;

    protected Rigidbody2D rb;

    protected virtual void Start()
    {
        player = Object.FindAnyObjectByType<PlayMovenments>();
        currentHp = maxHp;
        UpdateHpBar();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (player == null || rb == null) return;
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.linearVelocity = direction * enemyMoveSpeed;

            FlipEnemy();
        }
    }

    protected void FlipEnemy()
    {
        if (player != null)
        {
            transform.localScale = new Vector3(player.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
    }

    public void TakeDamage(float damge)
    {
        currentHp -= damge;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}
