using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Canvas hudCanvas;
    public Canvas gameOverCanvas;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;

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

    private void OnEnable()
    {
        PlayerController.onHealthChanged += UpdateHealthUI;
        GameManager.onGameOver += ShowGameOver;
        GameManager.onGameStart += ShowHUD;
        GameManager.onScoreChanged += UpdateScoreUI;
    }

    private void OnDisable()
    {
        PlayerController.onHealthChanged -= UpdateHealthUI;
        GameManager.onGameOver -= ShowGameOver;
        GameManager.onGameStart -= ShowHUD;
        GameManager.onScoreChanged -= UpdateScoreUI;
    }

    public void Init()
    {
        // Sahnedeki UI referanslarını yeniden bul
        hudCanvas = GameObject.Find("HUDCanvas")?.GetComponent<Canvas>();
        gameOverCanvas = GameObject.Find("GameOverCanvas")?.GetComponent<Canvas>();
        healthText = GameObject.Find("HealthText")?.GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("ScoreText")?.GetComponent<TextMeshProUGUI>();

        // New Run butonunu bağla
        Button newRunButton = GameObject.Find("NewRunButton")?.GetComponent<Button>();
        if (newRunButton != null)
        {
            newRunButton.onClick.RemoveAllListeners();
            newRunButton.onClick.AddListener(() =>
            {
                GameManager.Instance.LoadGameLevel();
            });
        }

        // Varsayılan görünüm: HUD açık, GameOver kapalı
        if (hudCanvas != null) hudCanvas.enabled = true;
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;

        Debug.Log("UIManager initialized with updated scene references.");
    }

    private void ShowHUD()
    {
        hudCanvas.enabled = true;
        gameOverCanvas.enabled = false;
    }

    private void ShowGameOver()
    {
        hudCanvas.enabled = false;
        gameOverCanvas.enabled = true;
    }

    private void UpdateHealthUI(float healthValue)
    {
        healthText.text = $"Health: {healthValue}";
    }

    private void UpdateScoreUI(int scoreValue)
    {
        scoreText.text = $"Score: {scoreValue}";
    }
}
