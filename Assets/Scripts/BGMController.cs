
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
        if (bgmAudioSource != null && !bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Play();
        }
    }

    // BGM 볼륨 조절
    public void SetBGMVolume(float volume)
    {
        SetMixerVolume("BGMVolume", volume);
    }

    // 효과음 볼륨 조절
    public void SetSFXVolume(float volume)
    {
        SetMixerVolume("SFXVolume", volume);
    }

    private void SetMixerVolume(string parameter, float volume)
    {
        if (audioMixer == null) return;

        volume = Mathf.Clamp01(volume);

        // 0은 음소거, 나머지는 데시벨로 변환
        float decibels = volume <= 0.0001f
            ? -80f
            : Mathf.Log10(volume) * 20f;

        audioMixer.SetFloat(parameter, decibels);
    }

    // 설정창을 열 때 음악 일시정지
    public void PauseBGM()
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.Pause();
        }

        isPaused = true;
    }

    // 설정창을 닫을 때 음악 재개
    public void ResumeBGM()
    {
        if (bgmAudioSource != null && isPaused)
        {
            bgmAudioSource.UnPause();
        }

        isPaused = false;
    }

    // 기존 기능 유지: 음악 처음부터 재생
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
