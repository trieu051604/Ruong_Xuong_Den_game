using System.Threading;
using UnityEngine;

public class PlayerConllision : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManagerPlayer audioManagerPlayer;
    private void OnTriggerEnter2D(Collider2D collision)
        {
        if (collision.gameObject.CompareTag("usb"))
        {
            Debug.Log("Collided with USB");
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Energy"))
        {
            gameManager.AddEnergy();
            Destroy(collision.gameObject);
            audioManagerPlayer.PlayCollectSound();
        }
    } 
}
