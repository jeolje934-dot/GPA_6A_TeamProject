using UnityEngine;

public class CoinSound : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // 1. 충돌한 대상이 "Player"(공)인지 확인
        if (other.CompareTag("Player"))
        {
            // 2. 공(Player)에 붙어있는 AudioSource를 찾아와서 소리 재생
            AudioSource playerAudio = other.GetComponent<AudioSource>();
            if (playerAudio != null && playerAudio.clip != null)
            {
                playerAudio.PlayOneShot(playerAudio.clip); // 겹쳐서 소리 재생 가능
            }

            // 3. 코인 오브젝트 즉시 삭제
            Destroy(gameObject);
        }
    }
}

