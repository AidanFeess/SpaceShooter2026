using UnityEngine;
using System.Collections;
using TMPro;

public class LevelController : MonoBehaviour
{
    [Header("Enemy Spawning")]
    public GameObject enemyPrefab;
    public float spawnX = 12f;
    public float minY = -4f;
    public float maxY = 4f;

    [Header("Level Settings")]
    public int enemiesUntilBoss = 20;
    public float baseSpawnInterval = 2f;
    public float baseEnemyFireRate = 2f;
    public float bossBaseHP = 100f;

    [Header("Prefabs")]
    public GameObject bossPrefab;

    [Header("UI")]
    public GameObject bossWarningScreen;
    public GameObject levelTransitionScreen;
    public GameObject winScreen;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI levelText;

    private int enemiesSpawned = 0;
    private bool bossSpawned = false;
    private bool levelComplete = false;
    private int currentLevel = 1;
    private int maxLevels = 2;

    void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        int enemiesToSpawn = enemiesUntilBoss * currentLevel; // doubles each level
        float spawnInterval = baseSpawnInterval / currentLevel; // faster each level

        while (enemiesSpawned < enemiesToSpawn && !bossSpawned)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnEnemy();
            enemiesSpawned++;
        }

        bossSpawned = true;
        StartCoroutine(SpawnBoss());
    }

    void SpawnEnemy()
    {
        float randomY = Random.Range(minY, maxY);
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(spawnX, randomY, 0f), Quaternion.identity);

        // scale fire rate with level
        Enemy e = enemy.GetComponent<Enemy>();
        if (e != null)
            e.fireRate = baseEnemyFireRate / currentLevel;
    }

    IEnumerator SpawnBoss()
    {
        if (bossWarningScreen != null)
        {
            bossWarningScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(2f);
            bossWarningScreen.SetActive(false);
        }

        GameObject bossObj = Instantiate(bossPrefab, new Vector3(spawnX, 0f, 0f), Quaternion.identity);

        // scale boss health with level
        Boss boss = bossObj.GetComponent<Boss>();
        if (boss != null)
            boss.health = bossBaseHP * currentLevel;
    }

    public void OnBossDied()
    {
        levelComplete = true;

        if (currentLevel >= maxLevels)
            StartCoroutine(ShowWinScreen());
        else
            StartCoroutine(LevelTransition());
    }

    IEnumerator LevelTransition()
    {
        if (levelTransitionScreen != null)
        {
            levelTransitionScreen.SetActive(true);
            yield return new WaitForSecondsRealtime(4f);
            levelTransitionScreen.SetActive(false);
        }

        // reset for next level
        currentLevel++;
        levelText.text = "Level " + currentLevel;
        enemiesSpawned = 0;
        bossSpawned = false;
        levelComplete = false;
        StartCoroutine(SpawnLoop());
    }

    IEnumerator ShowWinScreen()
    {
        yield return new WaitForSecondsRealtime(2f);
        if (winScreen != null)
        {
            winScreen.SetActive(true);
            Time.timeScale = 0f;
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.GetScore();
        }
    }
}