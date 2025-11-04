using Unity.VisualScripting;
using UnityEngine;

public class bow : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;

    private int maxAmmo;
    public int currentAmmo;
    private GameManager gameManager;
    [SerializeField] private AudioManagerPlayer audioManagerPlayer;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            maxAmmo = gameManager.ammoForThisLevel;
        }
        else
        {
            Debug.LogError("GameManager not found! Setting maxAmmo to 24 as a fallback.");

            maxAmmo = 24;
        }

        currentAmmo = maxAmmo;
    }

    void Update()
    {
        RotateBow();
        Shoot();
        Reload();
    }

    void RotateBow()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotateOffset);

        if (angle > 90 || angle < -90)
        {
            transform.localScale = new Vector3(1, -1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            audioManagerPlayer.PlayPlayerAttackSound();
        }
        
    }

    void Reload()
    {
        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        
    }
}
