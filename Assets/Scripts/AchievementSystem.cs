
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
        "100m에 도달하세요.",
        "500m에 도달하세요.",
        "1,000m에 도달하세요.",
        "5,000m에 도달하세요.",
        "10,000m에 도달하세요."
    };

    private static readonly int[] achievementTargets =
    {
        100, 500, 1000, 5000, 10000
    };

    public static float GetBestDistance()
    {
        return PlayerPrefs.GetFloat(BestDistanceKey, 0f);
    }

    // 게임 종료 시 최고 기록과 업적 저장
    public static void RecordDistance(float distance)
    {
        distance = Mathf.Max(0f, distance);

        float previousBest = GetBestDistance();

        if (distance > previousBest)
        {
            PlayerPrefs.SetFloat(BestDistanceKey, distance);
        }

        // 최고 기록을 기준으로 업적 달성 여부 확인
        float bestDistance = GetBestDistance();

        for (int i = 0; i < achievementTargets.Length; i++)
        {
            if (bestDistance >= achievementTargets[i])
            {
                PlayerPrefs.SetInt("AchievementUnlocked_" + i, 1);
            }
        }

        PlayerPrefs.Save();

        Debug.Log("최고 주행 거리: " + bestDistance);
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

    public static float GetProgress(int index)
    {
        return Mathf.Min(
            GetBestDistance(),
            achievementTargets[index]
        );
    }

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
