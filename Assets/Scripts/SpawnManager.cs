using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // Enemy prefab to spawn
    public GameObject[] upgradePickup;   // Upgrade pickup prefabs to spawn
    public float enemySpawnTime = 1f;         // Time interval between enemy spawns
    public float upgradeSpawnTime = 15f;    // Time interval between upgrade spawns
    public float upgradeYSpawnOffset = 0.5f;  // Y axis offset for upgrade spawns
    public float spawnOffset = 5f; // Distance from the visible screen edge
    public float spawnBoundsX = 10f;     // X-axis range around the player
    public float spawnBoundsZ = 10f;     // Z-axis range around the player

    private GameObject player;
    private PlayerController playerController;
    private bool isSpawnActive = false;
    private bool isUpgradeSpawnActive = false;
    private Coroutine enemySpawnCoroutine;
    private Coroutine upgradeSpawnCoroutine;

    private void OnEnable()
    {
        // Subscribe to the GameManager events
        GameManager.onGameStart += HandleGameStart;
        GameManager.onGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        // Unsubscribe from the GameManager events
        GameManager.onGameStart -= HandleGameStart;
        GameManager.onGameOver -= HandleGameOver;
    }

    private void HandleGameStart()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerController = player.GetComponent<PlayerController>();
        }

        if (enemySpawnCoroutine != null)
        {
            StopCoroutine(enemySpawnCoroutine);
        }

        if (upgradeSpawnCoroutine != null)
        {
            StopCoroutine(upgradeSpawnCoroutine);
        }

        StartSpawningEnemies();
        StartSpawningUpgrades();
    }

    private void HandleGameOver()
    {
        StopSpawningEnemies();
        StopSpawningUpgrades();
    }

    public void StopSpawningEnemies()
    {
        isSpawnActive = false;

        if (enemySpawnCoroutine != null)
        {
            StopCoroutine(enemySpawnCoroutine);
            enemySpawnCoroutine = null;
        }
    }

    public void StartSpawningEnemies()
    {
        isSpawnActive = true;
        enemySpawnCoroutine = StartCoroutine(StartSpawningEnemiesCoroutine());
    }

    IEnumerator StartSpawningEnemiesCoroutine()
    {
        while (isSpawnActive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }

    // Spawn one or more enemies outside the camera view
    public void SpawnEnemy(int spawnCount = 1)
    {
        if (enemyPrefab == null)
        {
            Debug.Log("There isnt any assigned enemies in " + name);
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    public void StopSpawningUpgrades()
    {
        isUpgradeSpawnActive = false;
        if (upgradeSpawnCoroutine != null)
        {
            StopCoroutine(upgradeSpawnCoroutine);
            upgradeSpawnCoroutine = null;
        }
    }

    public void StartSpawningUpgrades()
    {
        isUpgradeSpawnActive = true;
        upgradeSpawnCoroutine = StartCoroutine(StartSpawningUpgradesCoroutine());
    }
    IEnumerator StartSpawningUpgradesCoroutine()
    {
        while (isUpgradeSpawnActive)
        {
            SpawnUpgrade();
            yield return new WaitForSeconds(upgradeSpawnTime);
        }
    }

    public void SpawnUpgrade(int spawnCount = 1)
    {
        if (upgradePickup.Length == 0)
        {
            Debug.Log("There is no upgrade prefabs assigned to spawn in " + name);
            return;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Vector3 spawnOffset = new Vector3(0, upgradeYSpawnOffset, 0);
            GameObject upgradeToSpawn = upgradePickup[Random.Range(0, upgradePickup.Length)];
            Instantiate(upgradeToSpawn, spawnPosition + spawnOffset, Quaternion.identity);
        }
    }

    // Get a random spawn position that is outside the camera view, relative to the player
    Vector3 GetRandomSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        // Randomly choose one of the four directions (left, right, top, bottom)
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0: // Left
                spawnPosition.x = player.transform.position.x - spawnBoundsX - spawnOffset
        ;
                spawnPosition.z = Random.Range(player.transform.position.z - spawnBoundsZ, player.transform.position.z + spawnBoundsZ);
                break;
            case 1: // Right
                spawnPosition.x = player.transform.position.x + spawnBoundsX + spawnOffset
        ;
                spawnPosition.z = Random.Range(player.transform.position.z - spawnBoundsZ, player.transform.position.z + spawnBoundsZ);
                break;
            case 2: // Top
                spawnPosition.x = Random.Range(player.transform.position.x - spawnBoundsX, player.transform.position.x + spawnBoundsX);
                spawnPosition.z = player.transform.position.z + spawnBoundsZ + spawnOffset
        ;
                break;
            case 3: // Bottom
                spawnPosition.x = Random.Range(player.transform.position.x - spawnBoundsX, player.transform.position.x + spawnBoundsX);
                spawnPosition.z = player.transform.position.z - spawnBoundsZ - spawnOffset
        ;
                break;
        }

        spawnPosition.y = 0f; // Adjust if enemies need to be on ground level

        return spawnPosition;
    }
}
