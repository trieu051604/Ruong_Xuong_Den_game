using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerArrow : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 25f;
    [SerializeField] private float timeDestroy = 0.5f;
    [SerializeField] private float damage = 10f;
    void Start()
    {
        Destroy(gameObject, timeDestroy);
        
    }
    void Update()
    {
        MoveBullet();
}
    void MoveBullet()
    {
        transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("monster"))
        {
            Monster monster = collision.GetComponentInParent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }


}
