using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Canvas hudCanvas;
    public Canvas gameOverCanvas;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI scoreText;

    void Start()
    {
        if (hudCanvas != null) hudCanvas.enabled = true;
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;
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

        // Varsayılan görünüm: HUD açık, GameOver kapalı
        if (hudCanvas != null) hudCanvas.enabled = true;
        if (gameOverCanvas != null) gameOverCanvas.enabled = false;

        Debug.Log("UIManager initialized with updated scene references.");
    }

    public void OnNewRunButtonPressed()
    {
        GameManager.Instance.LoadGameLevel();
    }

    public void OnMainMenuButtonPressed()
    {
        GameManager.Instance.LoadMainMenu();
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
