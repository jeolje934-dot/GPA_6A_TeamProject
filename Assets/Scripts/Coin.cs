using UnityEngine;

public class Coin : MonoBehaviour
{
    // 띠링 사운드 클립 (인스펙터에서 할당)
    public AudioClip coinSound;

    [Range(0f, 1f)]
    public float soundVolume = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 상대방이 플레이어(공)인지 태그로 확인
        if (other.CompareTag("Player"))
        {
            // 사운드가 있다면 재생
            if (coinSound != null)
            {
                AudioSource.PlayClipAtPoint(coinSound, transform.position, soundVolume);
            }

            // TODO: 점수 증가 로직 (예: GameManager.Instance.AddScore(10);)

            // 코인 오브젝트 삭제 (소리가 끝까지 재생되도록 AudioSource.PlayClipAtPoint 사용 중)
            Destroy(gameObject);
        }
    }
}