
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class BuildSaveInitializer : MonoBehaviour
{
    // 빌드 최초 실행 여부를 확인하는 키
    private const string InitializedKey = "BuildSaveInitialized";

    private void Awake()
    {
#if !UNITY_EDITOR

        // 이미 초기화한 빌드라면 종료
        if (PlayerPrefs.GetInt(InitializedKey, 0) == 1)
            return;

        Debug.Log("최초 실행: 플레이어 저장 데이터 초기화");

        // 코인 초기화
        PlayerPrefs.DeleteKey("PlayerCoins");

        // 치장 아이템 초기화
        PlayerPrefs.DeleteKey("OwnedCosmeticItems");

        // 능력 구매 및 장착 기록 초기화
        PlayerPrefs.DeleteKey("OwnedAbilities");
        PlayerPrefs.DeleteKey("EquippedAbilities");

        // 최고 거리 초기화
        PlayerPrefs.DeleteKey("AchievementBestDistance");

        // 업적 달성 기록 초기화
        for (int i = 0; i < 5; i++)
        {
            PlayerPrefs.DeleteKey("AchievementUnlocked_" + i);
        }

        // 초기화 완료 기록
        PlayerPrefs.SetInt(InitializedKey, 1);
        PlayerPrefs.Save();

        Debug.Log("플레이어 저장 데이터 초기화 완료!");

#endif
    }
}
