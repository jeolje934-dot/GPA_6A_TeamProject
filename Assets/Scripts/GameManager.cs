
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("씬 설정")]
    public string titleSceneName = "TitleScene";

    [Header("코인")]
    public int coins = 0;

    // 기존 치장 아이템
    private HashSet<int> ownedItemIds = new HashSet<int>();

    // 신규: 구매한 능력
    private HashSet<int> ownedAbilityIds = new HashSet<int>();

    // 신규: 장착한 능력
    private HashSet<int> equippedAbilityIds = new HashSet<int>();

    [System.Serializable]
    private class ItemSaveData
    {
        public List<int> ids = new List<int>();
    }

    private const string CoinKey = "PlayerCoins";
    private const string ItemKey = "OwnedCosmeticItems";
    private const string AbilityKey = "OwnedAbilities";
    private const string EquippedAbilityKey = "EquippedAbilities";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }

    // 타이틀 화면으로 돌아가기
    public void ReturnToTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(titleSceneName);
    }

    // 코인 획득
    public void AddCoins(int amount)
    {
        if (amount <= 0)
            return;

        coins += amount;
        SaveData();

        Debug.Log("코인 획득! 현재 코인: " + coins);
    }

    // 코인 소비
    public bool SpendCoins(int amount)
    {
        if (amount <= 0 || coins < amount)
            return false;

        coins -= amount;
        SaveData();

        Debug.Log("코인 소비! 현재 코인: " + coins);
        return true;
    }

    // =========================
    // 치장 아이템 시스템 (기존)
    // =========================

    public bool AddItem(int itemId)
    {
        if (ownedItemIds.Contains(itemId))
        {
            Debug.Log("이미 보유한 아이템입니다: " + itemId);
            return false;
        }

        ownedItemIds.Add(itemId);
        SaveData();

        Debug.Log("치장 아이템 획득! ID: " + itemId);
        return true;
    }

    public bool HasItem(int itemId)
    {
        return ownedItemIds.Contains(itemId);
    }

    public List<int> GetOwnedItemIds()
    {
        return new List<int>(ownedItemIds);
    }

    // =========================
    // 능력 구매 시스템 (신규)
    // =========================

    // 능력 구매 여부
    public bool HasAbility(int abilityId)
    {
        return ownedAbilityIds.Contains(abilityId);
    }

    // 능력 구매
    public bool AddAbility(int abilityId)
    {
        if (ownedAbilityIds.Contains(abilityId))
            return false;

        ownedAbilityIds.Add(abilityId);
        SaveData();

        return true;
    }

    // 능력 장착 여부
    public bool HasEquippedAbility(int abilityId)
    {
        return equippedAbilityIds.Contains(abilityId);
    }

    // 능력 장착 / 해제
    public void SetAbilityEquipped(int abilityId, bool equipped)
    {
        if (!HasAbility(abilityId))
            return;

        if (equipped)
            equippedAbilityIds.Add(abilityId);
        else
            equippedAbilityIds.Remove(abilityId);

        SaveData();
    }

    // =========================
    // 데이터 저장
    // =========================

    private void SaveData()
    {
        PlayerPrefs.SetInt(CoinKey, coins);

        ItemSaveData cosmeticData = new ItemSaveData();
        cosmeticData.ids = new List<int>(ownedItemIds);

        ItemSaveData abilityData = new ItemSaveData();
        abilityData.ids = new List<int>(ownedAbilityIds);

        ItemSaveData equippedData = new ItemSaveData();
        equippedData.ids = new List<int>(equippedAbilityIds);

        PlayerPrefs.SetString(
            ItemKey,
            JsonUtility.ToJson(cosmeticData)
        );

        PlayerPrefs.SetString(
            AbilityKey,
            JsonUtility.ToJson(abilityData)
        );

        PlayerPrefs.SetString(
            EquippedAbilityKey,
            JsonUtility.ToJson(equippedData)
        );

        PlayerPrefs.Save();
    }

    // =========================
    // 데이터 불러오기
    // =========================

    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(CoinKey, 0);

        ownedItemIds = LoadIdSet(ItemKey);
        ownedAbilityIds = LoadIdSet(AbilityKey);
        equippedAbilityIds = LoadIdSet(EquippedAbilityKey);

        // 구매하지 않은 능력이 장착되는 오류 방지
        equippedAbilityIds.IntersectWith(ownedAbilityIds);
    }

    private HashSet<int> LoadIdSet(string key)
    {
        string json = PlayerPrefs.GetString(key, "");

        if (string.IsNullOrEmpty(json))
            return new HashSet<int>();

        ItemSaveData data =
            JsonUtility.FromJson<ItemSaveData>(json);

        if (data == null || data.ids == null)
            return new HashSet<int>();

        return new HashSet<int>(data.ids);
    }
}
