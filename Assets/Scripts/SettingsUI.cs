
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class SettingsUI : MonoBehaviour
{
    [Header("설정창")]
    public GameObject settingsPanel;

    [Header("볼륨 슬라이더")]
    public Slider bgmSlider;
    public Slider sfxSlider;

    [Header("사운드 매니저")]
    public BGMController bgmController;

    private bool isOpen = false;

    void Start()
    {
        settingsPanel.SetActive(false);

        bgmSlider.minValue = 0f;
        bgmSlider.maxValue = 1f;

        sfxSlider.minValue = 0f;
        sfxSlider.maxValue = 1f;

        bgmSlider.SetValueWithoutNotify(1f);
        sfxSlider.SetValueWithoutNotify(1f);

        bgmSlider.onValueChanged.AddListener(
            bgmController.SetBGMVolume);

        sfxSlider.onValueChanged.AddListener(
            bgmController.SetSFXVolume);
    }

    void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isOpen)
                CloseSettings();
            else
                OpenSettings();
        }
    }

    public void OpenSettings()
    {
        if (isOpen) return;

        isOpen = true;
        settingsPanel.SetActive(true);

        Time.timeScale = 0f;
        bgmController.PauseBGM();
    }

    public void CloseSettings()
    {
        if (!isOpen) return;

        settingsPanel.SetActive(false);

        Time.timeScale = 1f;
        bgmController.ResumeBGM();

        isOpen = false;
    }
}
