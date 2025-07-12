using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameData data;
    public GameObject player;
    public GameObject spawnManager;
    public bool isGameActive = false;

    public delegate void OnScoreChanged(int score);
    public static event OnScoreChanged onScoreChanged;
    public static event Action onGameStart;
    public static event Action onGameOver;

    void OnEnable()
    {
        PlayerController.onPlayerDied += GameOver;
    }

    void OnDisable()
    {
        PlayerController.onPlayerDied -= GameOver;
    }

    // Singleton pattern to ensure only one instance of GameManager exists
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Init()
    {
        // Find and assign references to player and spawn manager
        player = GameObject.FindWithTag("Player");
        spawnManager = GameObject.Find("SpawnManager");

        Debug.Log("GameManager initialized and game started.");
    }
    public void GameStart()
    {
        isGameActive = true;
        ResetLevel(); // Reset game data when starting a new game
        onGameStart?.Invoke(); // Notify that the game has started
        Debug.Log("Game Started! Score reset to: " + data.score);
    }

    public void GameOver()
    {
        isGameActive = false;
        onGameOver?.Invoke(); // Notify that the game is over
        Debug.Log("Game Over! Final Score: " + data.score);
    }

    public void LoadGameLevel()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadLevel("Game");
    }

    public void LoadMainMenu()
    {
        LoadLevel("MainMenu");
    }

    // This method is called when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Init(); // Initialize GameManager with updated scene references

        if (scene.name == "Game")
        {
            GameStart();
        }
    }


    public void ResetLevel()
    {
        // Reset game data to initial state
        SetScore(0); 
        // Reset player state
        if (player != null)
        {
            PlayerController playerController = player.GetComponent<PlayerController>();
            SpawnManager spawnManagerScript = spawnManager.GetComponent<SpawnManager>();
            if (playerController != null)
            {
                playerController.ResetPlayer(); // Reset player state
                //Stop spawning enemies
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void AddScore(int amount)
    {
        data.score += amount;
        UpdateScoreUI(data.score);
    }

    public void SetScore(int score)
    {
        data.score = score;
        UpdateScoreUI(data.score);
    }

    public void UpdateScoreUI(int score)
    {
        onScoreChanged?.Invoke(score);
    }
}
