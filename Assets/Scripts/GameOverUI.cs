
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("업적 팝업")]
    [SerializeField] private AchievementPopupController achievementPopup;

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
#if UNITY_EDITOR
        // 개발자 테스트 키

        // F7: 업적 기록 초기화
        if (Input.GetKeyDown(KeyCode.F7))
        {
            AchievementSystem.ResetAchievements();
        }

        // F8~F12: 업적 팝업 테스트
        if (achievementPopup != null)
        {
            if (Input.GetKeyDown(KeyCode.F8))
                achievementPopup.ShowAchievement(0);

            if (Input.GetKeyDown(KeyCode.F9))
                achievementPopup.ShowAchievement(1);

            if (Input.GetKeyDown(KeyCode.F10))
                achievementPopup.ShowAchievement(2);

            if (Input.GetKeyDown(KeyCode.F11))
                achievementPopup.ShowAchievement(3);

            if (Input.GetKeyDown(KeyCode.F12))
                achievementPopup.ShowAchievement(4);
        }
#endif

        if (isGameOver)
            return;

        survivalTime += Time.deltaTime;

        UpdateDistance();

        // 실제 거리 달성 확인
        List<int> newlyUnlocked =
            AchievementSystem.CheckDistance(distanceTravelled);

        foreach (int index in newlyUnlocked)
        {
            if (achievementPopup != null)
            {
                achievementPopup.ShowAchievement(index);
            }
        }

        UpdateGameUI();
    }

    // 실제 이동 거리 누적
    private void UpdateDistance()
    {
        if (player == null)
            return;

        Vector3 currentPosition = player.position;

        distanceTravelled += Mathf.Max(
            0f,
            currentPosition.z - lastPosition.z
        );

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

        // 업적 및 최고 거리 저장
        AchievementSystem.RecordDistance(distanceTravelled);

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
