using UnityEngine;

public class AchievementPanelController : MonoBehaviour
{
    [Header("업적 패널")]
    public GameObject achievementPanel;

    private AchievementItemUI[] achievementItems;

    void Awake()
    {
        if (achievementPanel != null)
        {
            achievementItems =
                achievementPanel.GetComponentsInChildren<AchievementItemUI>(true);

            achievementPanel.SetActive(false);
        }
    }



    public void CloseAchievement()
    {
        Debug.Log("업적 닫기 버튼 실행됨!");

        if (achievementPanel != null)
        {
            achievementPanel.SetActive(false);
        }
    }

    public void OpenAchievement()
    {
        if (achievementPanel == null)
            return;

        achievementPanel.SetActive(true);

        RefreshAllAchievements();
    }

    //public void CloseAchievement()
    //{
       // if (achievementPanel != null)
       // {
           // achievementPanel.SetActive(false);
       // }
    //}

    public void RefreshAllAchievements()
    {
        if (achievementItems == null)
            return;

        foreach (AchievementItemUI item in achievementItems)
        {
            if (item != null)
            {
                item.Refresh();
            }
        }
    }
}