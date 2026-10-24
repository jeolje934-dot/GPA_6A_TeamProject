
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallDetector : MonoBehaviour
{
    [Header("낙하 설정")]
    [SerializeField] private float fallLimit = -5f;

    [Header("게임 오버 UI")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameOverUI gameOverUI;

    private bool isGameOver = false;

    private void Start()
    {
        Time.timeScale = 1f;

        isGameOver = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (isGameOver)
            return;

        if (transform.position.y < fallLimit)
        {
            GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isGameOver)
            return;

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        Debug.Log("게임 오버 발생!");

        Time.timeScale = 0f;

        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }
        else if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("게임 오버 UI가 연결되지 않았습니다.");
        }
    }

    public void RestartGame()
    {
        Debug.Log("다시 시작 버튼 클릭!");

        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
