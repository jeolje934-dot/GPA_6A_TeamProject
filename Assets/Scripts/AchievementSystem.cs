
using System.Collections.Generic;
using UnityEngine;

public static class AchievementSystem
{
    private const string BestDistanceKey = "AchievementBestDistance";

    private static readonly string[] achievementNames =
    {
        "첫 번째 주행",
        "초보 러너",
        "숙련된 러너",
        "장거리 주자",
        "전설의 러너"
    };

    private static readonly string[] achievementDescriptions =
    {
        "100m에 도달했습니다!",
        "500m에 도달했습니다!",
        "1,000m에 도달했습니다!",
        "5,000m에 도달했습니다!",
        "10,000m에 도달했습니다!"
    };

    private static readonly int[] achievementTargets =
    {
        100, 500, 1000, 5000, 10000
    };

    // 최고 주행 거리
    public static float GetBestDistance()
    {
        return PlayerPrefs.GetFloat(BestDistanceKey, 0f);
    }

    // 현재 거리를 확인하고 새롭게 달성한 업적 반환
    public static List<int> CheckDistance(float distance)
    {
        List<int> newlyUnlocked = new List<int>();

        for (int i = 0; i < achievementTargets.Length; i++)
        {
            string key = "AchievementUnlocked_" + i;

            if (distance >= achievementTargets[i] &&
                PlayerPrefs.GetInt(key, 0) == 0)
            {
                PlayerPrefs.SetInt(key, 1);
                newlyUnlocked.Add(i);

                Debug.Log("업적 달성: " + achievementNames[i]);
            }
        }

        if (newlyUnlocked.Count > 0)
        {
            PlayerPrefs.Save();
        }

        return newlyUnlocked;
    }

    // 게임 종료 시 최고 기록 저장
    public static void RecordDistance(float distance)
    {
        float previousBest = GetBestDistance();

        if (distance > previousBest)
        {
            PlayerPrefs.SetFloat(BestDistanceKey, distance);
        }

        // 게임오버 시에도 최종 거리 기준으로 업적 확인
        CheckDistance(distance);

        PlayerPrefs.Save();

        Debug.Log("최고 주행 거리: " + GetBestDistance());
    }

    public static string GetName(int index)
    {
        return achievementNames[index];
    }

    public static string GetDescription(int index)
    {
        return achievementDescriptions[index];
    }

    public static int GetTarget(int index)
    {
        return achievementTargets[index];
    }

    // 업적 진행도
    public static float GetProgress(int index)
    {
        return Mathf.Min(
            GetBestDistance(),
            achievementTargets[index]
        );
    }

    // 업적 달성 여부
    public static bool IsUnlocked(int index)
    {
        return PlayerPrefs.GetInt(
            "AchievementUnlocked_" + index, 0
        ) == 1;
    }

    public static int GetAchievementCount()
    {
        return achievementTargets.Length;
    }
}
