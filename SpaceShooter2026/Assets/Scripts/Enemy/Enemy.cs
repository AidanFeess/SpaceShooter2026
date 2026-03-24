using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 3;
    public int scoreValue = 100;
    private float currentHealth;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;
    public float fireRate = 2f;
    private float nextFireTime = 0f;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip deathSound;
    public AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        nextFireTime = Time.time + Random.Range(0f, fireRate); // stagger first shot
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
        // destroy off screen enemies
        if (transform.position.x < -15f)
        {
            Destroy(gameObject);
        }
    }

    void Shoot()
    {
        if (bulletPrefab != null && bulletSpawnPoint != null)
        {
            Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
            if (shootSound != null)
                audioSource.PlayOneShot(shootSound);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // TODO: play explosion effect + sound
        ScoreManager.Instance.AddScore(scoreValue);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().TakeDamage(1);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damage);
            Destroy(other.gameObject);
        }
    }
}