using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    private bool isDead = false;
    public static int coinCount = 0; // 획득한 코인/재화 누적 변수

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        // 장애물 충돌 시 게임 오버[cite: 8, 11]
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }

    // Is Trigger가 켜진 코인과 닿았을 때 처리
    private void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        // 태그가 "Coin"인 오브젝트와 닿았을 때[cite: 8, 11]
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount++;
            Debug.Log("코인 획득! 현재 코인: " + coinCount);

            // 코인 오브젝트 비활성화 (먹은 효과)[cite: 11]
            other.gameObject.SetActive(false);
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("게임 오버!");
        Invoke("RestartGame", 1.0f);
    }

    void RestartGame()
    {
        coinCount = 0; // 재시작 시 초기화
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


