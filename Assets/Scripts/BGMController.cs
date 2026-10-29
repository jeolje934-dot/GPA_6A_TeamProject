
using UnityEngine;
using UnityEngine.Audio;

public class BGMController : MonoBehaviour
{
    [Header("배경음악")]
    public AudioSource bgmAudioSource;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;

    private bool isPaused = false;

    void Start()
    {
        // 게임 시작 시 배경음악 재생
        if (bgmAudioSource != null)
        {
            bgmAudioSource.loop = true;

            if (!bgmAudioSource.isPlaying)
            {
                bgmAudioSource.Play();
            }
        }
    }

    public void SetBGMVolume(float volume)
    {
        SetMixerVolume("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        SetMixerVolume("SFXVolume", volume);
    }

    private void SetMixerVolume(string parameter, float volume)
    {
        if (audioMixer == null) return;

        volume = Mathf.Clamp01(volume);

        float decibels = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat(parameter, decibels);
    }

    // 설정창에서 음악 일시정지
    public void PauseBGM()
    {
        if (bgmAudioSource != null && bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Pause();
            isPaused = true;
        }
    }

    // 설정창을 닫으면 음악 재개
    public void ResumeBGM()
    {
        if (bgmAudioSource != null && isPaused)
        {
            bgmAudioSource.UnPause();
            isPaused = false;
        }
    }

    // 장애물 충돌 또는 게임 오버 시 음악 정지
    public void StopBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
        }

        isPaused = false;
    }

    // 재시작할 때 음악 처음부터 재생
    public void RestartBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Stop();
            bgmAudioSource.Play();
            isPaused = false;
        }
    }
}
