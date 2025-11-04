using UnityEngine;
using UnityEngine.UI;

public abstract class Monster : MonoBehaviour
{
    private enum AIState { Patrol, Chase }
    private AIState currentState;

    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float patrolRange = 4f;

    private Vector2 spawnPoint;
    private Vector2 patrolTarget;

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
        player = FindAnyObjectByType<PlayMovenments>();
        currentHp = maxHp;
        UpdateHpBar();
        rb = GetComponent<Rigidbody2D>();
        currentState = AIState.Patrol;
        spawnPoint = transform.position;
        SetNewPatrolTarget();
    }

    protected virtual void Update()
    {
        CheckForPlayer();
        FlipEnemy();
    }

    protected virtual void FixedUpdate()
    {
        if (player == null || rb == null) return;

        switch (currentState)
        {
            case AIState.Patrol:
                MovePatrol();
                break;
            case AIState.Chase:
                MoveChase();
                break;
        }
    }

    private void CheckForPlayer()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
        currentState = distanceToPlayer < detectionRange ? AIState.Chase : AIState.Patrol;
    }

    private void MovePatrol()
    {
        Vector2 direction = (patrolTarget - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * enemyMoveSpeed;

        if (Vector2.Distance(transform.position, patrolTarget) < 0.5f)
        {
            SetNewPatrolTarget();
        }
    }

    private void MoveChase()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        rb.linearVelocity = direction * enemyMoveSpeed;
    }

    private void SetNewPatrolTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * patrolRange;
        patrolTarget = spawnPoint + randomOffset;
    }

    protected void FlipEnemy()
    {
        if (rb.linearVelocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (rb.linearVelocity.x > 0.1f)
        {
            transform.localScale = new Vector3(1, 1, 1);
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
