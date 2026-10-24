
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

    // 보유한 치장 아이템 ID
    private HashSet<int> ownedItemIds = new HashSet<int>();

    [System.Serializable]
    private class ItemSaveData
    {
        public List<int> ids = new List<int>();
    }

    private const string CoinKey = "PlayerCoins";
    private const string ItemKey = "OwnedCosmeticItems";

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
        if (amount <= 0) return;

        coins += amount;
        SaveData();

        Debug.Log("코인 획득! 현재 코인: " + coins);
    }

    // 치장 아이템 획득
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

    // 아이템 보유 여부
    public bool HasItem(int itemId)
    {
        return ownedItemIds.Contains(itemId);
    }

    // 인벤토리용 아이템 목록
    public List<int> GetOwnedItemIds()
    {
        return new List<int>(ownedItemIds);
    }

    // 저장
    private void SaveData()
    {
        PlayerPrefs.SetInt(CoinKey, coins);

        ItemSaveData data = new ItemSaveData();
        data.ids = new List<int>(ownedItemIds);

        PlayerPrefs.SetString(
            ItemKey,
            JsonUtility.ToJson(data)
        );

        PlayerPrefs.Save();
    }

    // 불러오기
    private void LoadData()
    {
        coins = PlayerPrefs.GetInt(CoinKey, 0);

        string json = PlayerPrefs.GetString(ItemKey, "");

        if (!string.IsNullOrEmpty(json))
        {
            ItemSaveData data =
                JsonUtility.FromJson<ItemSaveData>(json);

            if (data != null && data.ids != null)
            {
                ownedItemIds = new HashSet<int>(data.ids);
            }
        }
    }
}
