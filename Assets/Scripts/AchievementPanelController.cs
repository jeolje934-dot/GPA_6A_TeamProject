
using UnityEngine;

public class AchievementPanelController : MonoBehaviour
{
    public GameObject achievementPanel;

    void Start()
    {
        achievementPanel.SetActive(false);
    }

    public void OpenAchievement()
    {
        achievementPanel.SetActive(true);
    }

    public void CloseAchievement()
    {
        achievementPanel.SetActive(false);
    }
}
