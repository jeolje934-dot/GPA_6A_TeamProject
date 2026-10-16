
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerFallDetector : MonoBehaviour
{
    [SerializeField] private float fallLimit = -5f;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameOverUI gameOverUI;

    private bool isGameOver = false;

    void Start()
    {
        Time.timeScale = 1f;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver)
            return;

        if (transform.position.y < fallLimit)
        {
            GameOver();
        }
    }

    void OnCollisionEnter(Collision collision)
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

        if (gameOverUI != null)
            gameOverUI.ShowGameOver();

        Time.timeScale = 0f;

        Debug.Log("게임오버!");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
