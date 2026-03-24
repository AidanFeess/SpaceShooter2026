using UnityEngine;
using System.Collections;

public class Boss : MonoBehaviour
{
    [Header("Stats")]
    public float health;
    public int scoreValue = 1000;

    [Header("Movement")]
    public float moveSpeed = 2f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Shooting")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 1f;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioClip damageSound;
    public AudioClip deathSound;
    public AudioSource audioSource;

    private float targetY;
    private bool dead;
    private LevelController levelController;

    void Start()
    {
        levelController = FindFirstObjectByType<LevelController>();
        StartCoroutine(ShootLoop());
        PickNewTarget();
    }

    void Update()
    {
        // move toward target Y
        float newY = Mathf.MoveTowards(transform.position.y, targetY, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        if (Mathf.Abs(transform.position.y - targetY) < 0.1f)
            PickNewTarget();

        // move left until it reaches its arena position
        if (transform.position.x > 4f)
            transform.position = new Vector3(transform.position.x - moveSpeed * Time.deltaTime, transform.position.y, transform.position.z);

    }

    void PickNewTarget()
    {
        targetY = Random.Range(minY, maxY);
    }

    IEnumerator ShootLoop()
    {
        while (!dead)
        {
            yield return new WaitForSeconds(fireRate);
            if (bulletPrefab != null && firePoint != null)
                Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                if (shootSound != null)
                    audioSource.PlayOneShot(shootSound);
        }
    }

    public void TakeDamage(float amount)
    {
        if (damageSound != null && !dead)
            audioSource.PlayOneShot(damageSound);
        health -= amount;
        if (health <= 0)
            Die();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(other.GetComponent<Bullet>().damage);
            Destroy(other.gameObject);
        }
    }

    void Die()
    {
        if (dead == true)
            return;
        dead = true;
        ScoreManager.Instance.AddScore(scoreValue);
        StartCoroutine(DeathSequence());
    }


    IEnumerator DeathSequence()
    {
        // freeze the boss
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        this.enabled = false; // disables Update so it stops moving and shooting

        // play explosion sound
        if (deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
            yield return new WaitForSeconds(deathSound.length);
        }

        levelController.OnBossDied();
        Destroy(gameObject);
    }
}