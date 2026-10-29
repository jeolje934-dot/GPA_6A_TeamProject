
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private bool isDead = false;

    [Header("Game Over UI")]
    [SerializeField] private GameOverUI gameOverUI;

    [SerializeField] private BGMController bgmController;

    private void Start()
    {
        // 게임 시작 시 일시정지 해제
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

        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager를 찾을 수 없습니다.");
            return;
        }

        // 코인 획득
        if (other.CompareTag("Coin"))
        {
            GameManager.Instance.AddCoins(1);

            // 게임오버 화면에 획득량 전달
            if (gameOverUI != null)
            {
                gameOverUI.AddCoins(1);
            }

            // 획득한 코인 비활성화
            other.gameObject.SetActive(false);
            return;
        }

        // 치장 아이템 획득
        CosmeticPickup cosmetic =
            other.GetComponent<CosmeticPickup>();

        if (cosmetic != null)
        {
            cosmetic.Collect(GameManager.Instance);
        }
    }


    public void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("게임 오버!");

        if (bgmController != null)
        {
            bgmController.StopBGM();
        }

        if (gameOverUI != null)
        {
            gameOverUI.ShowGameOver();
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        // 현재 씬 다시 불러오기
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }
}
