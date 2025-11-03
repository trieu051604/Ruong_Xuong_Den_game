using UnityEngine;
using UnityEngine.UI;

public class PlayMovenments : MonoBehaviour
{
    public Rigidbody2D rb;
    public float speed = 5f;

    public Animator animator;
    private Vector2 movement;
    public SpriteRenderer spriteRenderer;
    private bool isAttacking = false;
    [SerializeField] private float maxHp = 50f;
    private float currentHp;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameManager gameManager;



    public static PlayMovenments Instance { get; private set; }

    private void Start()
    {
        currentHp = maxHp;
        UpdateHpBar();
    }
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x != 0)
        {
            spriteRenderer.flipX = movement.x < 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameManager.PauseGameMenu();
        }
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
        }
    }

    public void TakeDamge(float damage)
    {
        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameManager.GameOverMenu();
        //Destroy(gameObject);
    }
    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp;
        }
    }
}

