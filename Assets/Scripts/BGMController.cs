using UnityEngine;

public class BGMController : MonoBehaviour
{
    public AudioSource bgmAudioSource;

    void Start()
    {
        if (bgmAudioSource != null && !bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Play();
        }
    }

    // 외부(장애물 충돌 스크립트 등)에서 부딪혔을 때 호출할 수 있는 함수
    public void RestartBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop(); // 현재 재생 중인 음악을 멈추고
            bgmAudioSource.Play(); // 처음부터 다시 재생
        }
    }
}