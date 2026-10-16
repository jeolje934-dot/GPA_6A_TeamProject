
using UnityEngine;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text statsText;

    [Header("Player")]
    [SerializeField] private Transform player;

    private Vector3 startPosition;
    private float survivalTime;
    private int totalCoins;
    private bool isGameOver;

    void Start()
    {
        if (player != null)
            startPosition = player.position;

        survivalTime = 0f;
        totalCoins = 0;
        isGameOver = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver)
            survivalTime += Time.deltaTime;
    }

    public void AddCoins(int amount)
    {
        if (!isGameOver)
            totalCoins += amount;
    }

    public void ShowGameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        float distance = 0f;

        if (player != null)
        {
            Vector3 current = player.position;
            Vector3 start = startPosition;

            current.y = 0f;
            start.y = 0f;

            distance = Vector3.Distance(start, current);
        }

        int minutes = Mathf.FloorToInt(survivalTime / 60f);
        int seconds = Mathf.FloorToInt(survivalTime % 60f);

        if (statsText != null)
        {
            statsText.text =
                $"• 최종 이동 거리: {distance:N0}m\n" +
                $"• 생존 시간: {minutes:00}분 {seconds:00}초\n" +
                $"• 획득한 재화: +{totalCoins} 코인";
        }

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }
}
