using UnityEngine;

public class attack_monster : MonoBehaviour
{
    private Vector3 movementDirection;

    void Start()
    {
        Destroy(gameObject, 5);
    }

    void Update()
    {
        if (movementDirection == Vector3.zero) return;
        transform.position += movementDirection * Time.deltaTime;
    }

    public void SetMovementDirection(Vector3 direction)
    {
        movementDirection = direction;
    }
}
