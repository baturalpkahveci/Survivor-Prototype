using System.Collections;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;       // Enemy prefab to spawn
    public float spawnTime = 1f;         // Time interval between spawns
    public float spawnOffset = 5f; // Distance from the visible screen edge
    public float spawnBoundsX = 10f;     // X-axis range around the player
    public float spawnBoundsZ = 10f;     // Z-axis range around the player

    private GameObject player;
    private PlayerController playerController;
    private bool isSpawnActive = false;
    private Coroutine spawnCoroutine;

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

    void Start()
    {
        // Find the player GameObject by its tag
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
        isSpawnActive = GameManager.Instance.isGameActive;
        // Start the enemy spawning coroutine
        StartCoroutine(StartSpawningEnemies());
    }

    private void HandleGameStart()
    {
        isSpawnActive = true;
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
        spawnCoroutine = StartCoroutine(StartSpawningEnemies());
    }

    private void HandleGameOver()
    {
        StartCoroutine(StopSpawningEnemies());
    }

    private IEnumerator StopSpawningEnemies()
    {
        isSpawnActive = false;
        yield return new WaitForSeconds(0.5f);

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    IEnumerator StartSpawningEnemies()
    {
        while (isSpawnActive)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnTime);
        }
    }

    // Spawn one or more enemies outside the camera view
    void SpawnEnemy(int spawnCount = 1)
    {
        for (int i = 0; i < spawnCount; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
