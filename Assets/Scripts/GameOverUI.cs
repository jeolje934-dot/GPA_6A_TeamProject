
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("게임 진행 UI")]
    [SerializeField] private TMP_Text currentCoinsText;
    [SerializeField] private TMP_Text currentDistanceText;

    [Header("게임오버 UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text statsText;

    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Scene")]
    [SerializeField] private string titleSceneName = "TitleScene";

    private Vector3 startPosition;
    private Vector3 lastPosition;

    private float survivalTime;
    private float distanceTravelled;
    private int totalCoins;
    private bool isGameOver;

    void Start()
    {
        Time.timeScale = 1f;

        if (player != null)
        {
            startPosition = player.position;
            lastPosition = player.position;
        }

        survivalTime = 0f;
        distanceTravelled = 0f;
        totalCoins = 0;
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        UpdateGameUI();
    }

    void Update()
    {
        if (isGameOver)
            return;

        survivalTime += Time.deltaTime;

        UpdateDistance();
        UpdateGameUI();
    }

    // 실제 이동 거리 누적
    private void UpdateDistance()
    {
        if (player == null)
            return;

        Vector3 currentPosition = player.position;

        Vector3 currentFlat = new Vector3(
            currentPosition.x, 0f, currentPosition.z
        );

        Vector3 lastFlat = new Vector3(
            lastPosition.x, 0f, lastPosition.z
        );

        distanceTravelled +=
            Vector3.Distance(lastFlat, currentFlat);

        lastPosition = currentPosition;
    }

    // 게임 중 코인과 거리 표시
    private void UpdateGameUI()
    {
        if (currentCoinsText != null)
        {
            currentCoinsText.text = "코인: " + totalCoins;
        }

        if (currentDistanceText != null)
        {
            currentDistanceText.text =
                "거리: " + Mathf.FloorToInt(distanceTravelled) + "m";
        }
    }

    // 코인 획득
    public void AddCoins(int amount)
    {
        if (isGameOver || amount <= 0)
            return;

        totalCoins += amount;
        UpdateGameUI();
    }

    // 게임오버
    public void ShowGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);

        if (statsText != null)
        {
            statsText.text =
                $"• 최종 이동 거리: {Mathf.FloorToInt(distanceTravelled):N0}m\n" +
                $"• 생존 시간: {minutes:00}분 {seconds:00}초\n" +
                $"• 획득한 재화: +{totalCoins} 코인";
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void GoToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }
}
