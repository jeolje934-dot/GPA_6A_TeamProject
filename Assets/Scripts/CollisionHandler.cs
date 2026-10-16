
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private bool isDead = false;

    public static int coinCount = 0;

    [Header("Game Over UI")]
    [SerializeField] private GameOverUI gameOverUI;

    private void Start()
    {
        coinCount = 0;
        Time.timeScale = 1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // 장애물에 부딪히면 게임 오버
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // 코인 획득
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount++;

            // 게임오버 화면에 코인 획득량 전달
            if (gameOverUI != null)
            {
                gameOverUI.AddCoins(1);
            }

            Debug.Log("코인 획득! 현재 코인: " + coinCount);

            // 획득한 코인 비활성화
            other.gameObject.SetActive(false);
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("게임 오버!");

        // 게임오버 화면 표시
        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }

        // 게임 정지
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        coinCount = 0;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
