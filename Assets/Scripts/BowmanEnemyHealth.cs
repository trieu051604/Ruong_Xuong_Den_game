using UnityEngine;
using UnityEngine.UI; // ✅ Quan trọng: thêm dòng này để dùng Image UI

public class BowmanEnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("Health Bar Settings")]
    public Transform healthBarRoot; // object chứa Canvas thanh máu
    public Image fillImage;         // ✅ đây là hình đỏ của thanh máu
    public Vector3 offset = new Vector3(0, 1.2f, 0);

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Cập nhật vị trí thanh máu luôn nằm trên đầu quái
        if (healthBarRoot != null)
        {
            healthBarRoot.position = transform.position + offset;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (fillImage != null)
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " đã chết!");
        Destroy(gameObject);
    }
}
