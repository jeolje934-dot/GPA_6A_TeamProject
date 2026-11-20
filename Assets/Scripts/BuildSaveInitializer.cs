
using UnityEngine;

public class BuildSaveInitializer : MonoBehaviour
{
    // 새 빌드를 만들 때 이 값을 변경합니다.
    private const string BuildVersion = "1.0.1";

    private const string InitializedVersionKey = "BuildSaveInitializedVersion";

    private void Awake()
    {
#if !UNITY_EDITOR
        string savedVersion = PlayerPrefs.GetString(
            InitializedVersionKey, ""
        );

        if (savedVersion == BuildVersion)
            return;

        Debug.Log("새 빌드 감지: 플레이어 데이터 초기화");

        // 코인 및 아이템
        PlayerPrefs.DeleteKey("PlayerCoins");
        PlayerPrefs.DeleteKey("OwnedCosmeticItems");
        PlayerPrefs.DeleteKey("OwnedAbilities");
        PlayerPrefs.DeleteKey("EquippedAbilities");

        // 업적
        PlayerPrefs.DeleteKey("AchievementBestDistance");

        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey("AchievementUnlocked_" + i);
        }

        // 초기화 완료 버전 저장
        PlayerPrefs.SetString(InitializedVersionKey, BuildVersion);
        PlayerPrefs.Save();

        Debug.Log("플레이어 데이터 초기화 완료!");
#endif
    }
}
