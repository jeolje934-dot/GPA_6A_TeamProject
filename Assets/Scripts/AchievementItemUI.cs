
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementItemUI : MonoBehaviour
{
    [Header("업적 번호 (0~4)")]
    [SerializeField] private int achievementIndex;

    [Header("UI 연결")]
    [SerializeField] private TMP_Text achievementNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text progressText;
    [SerializeField] private Slider progressBar;

    public void Refresh()
    {
        if (achievementIndex < 0 ||
            achievementIndex >= AchievementSystem.GetAchievementCount())
            return;

        int target = AchievementSystem.GetTarget(achievementIndex);

        float progress = AchievementSystem.GetProgress(achievementIndex);

        bool unlocked = AchievementSystem.IsUnlocked(achievementIndex);

        if (achievementNameText != null)
        {
            achievementNameText.text =
                (unlocked ? "V " : "") +
                AchievementSystem.GetName(achievementIndex);
        }

        if (descriptionText != null)
        {
            descriptionText.text =
                unlocked ? "목표를 달성했습니다!" :
                AchievementSystem.GetDescription(achievementIndex);
        }

        if (progressText != null)
        {
            progressText.text =
                $"{Mathf.FloorToInt(progress):N0} / {target:N0}m";
        }

        if (progressBar != null)
        {
            progressBar.minValue = 0;
            progressBar.maxValue = target;
            progressBar.value = progress;
            progressBar.interactable = false;
        }
    }

    private void OnEnable()
    {
        Refresh();
    }
}
