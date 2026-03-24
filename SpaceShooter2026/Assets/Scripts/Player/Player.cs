using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class Player : MonoBehaviour 
{
    // Public
    [Header("Game UI")]
    public Slider healthSlider;

    [Header("Bullet Stuff")]
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint;

    [Header("Screens")]
    public GameObject gameOverScreen;
    public GameObject gameplayScreen;
    public GameObject bossWarningScreen;
    public GameObject levelTransitionScreen;
    public GameObject gameWinScreen;

    public TextMeshProUGUI gameOverText;

    [Header("Hearts")]
    public Image heart1;
    public Image heart2;
    public Image heart3;

    [Header("Audio")]
    public AudioClip shootSound;
    public AudioSource audioSource;

    // Private
    private float health;
    private float fireRate;
    private float nextFireTime;
    private float lives;

    private void Start()
    {
        health = 1.0f;
        lives = 3;
        fireRate = 0.2f; // secs between shots
        gameplayScreen.SetActive(true);
        gameOverScreen.SetActive(false);
        bossWarningScreen.SetActive(false);
        levelTransitionScreen.SetActive(false);
        gameWinScreen.SetActive(false);
    }

    private void Update()
    {
        healthSlider.value = health;

        if (Keyboard.current.spaceKey.isPressed && Time.time >= nextFireTime && health > 0)
        {
            nextFireTime = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
        if (shootSound != null)
            audioSource.PlayOneShot(shootSound);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    void Die()
    {
        if (lives > 1)
        {
            Time.timeScale = 0f;
            lives--;
            UpdateHearts();
            // 1 sec of flashing
            StartCoroutine(GetComponent<FlashEffect>().Flash(2f, 0.2f));
            StartCoroutine(ResumeAfterFlash(1f)); // restart game
            return;
        }
        gameplayScreen.SetActive(false);
        gameOverScreen.SetActive(true);
        bossWarningScreen.SetActive(false);
        levelTransitionScreen.SetActive(false);
        gameWinScreen.SetActive(false);
        gameOverText.text = "Game Over!\nYou Died\nFinal Score: " + ScoreManager.Instance.GetScore();
        
    }

    IEnumerator ResumeAfterFlash(float delay)
    {
        yield return new WaitForSecondsRealtime(delay);
        health = 1f;
        Time.timeScale = 1f;
    }

    void UpdateHearts()
    {
        heart1.color = lives >= 1 ? Color.white : Color.black;
        heart2.color = lives >= 2 ? Color.white : Color.black;
        heart3.color = lives >= 3 ? Color.white : Color.black;
    }
}
