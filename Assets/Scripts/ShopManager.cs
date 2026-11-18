
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("상점 UI")]
    [SerializeField] private GameObject shopPanel;

    // 상점 열기
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        RefreshShop();
    }

    // 상점 닫기
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    // 능력 구매 및 장착 / 해제
    public void BuyOrEquipAbility(int abilityId, int price)
    {
        GameManager gm = GameManager.Instance;

        if (gm == null)
            return;

        // 이미 구매한 능력이라면 장착 또는 해제
        if (gm.HasAbility(abilityId))
        {
            bool equipped = gm.HasEquippedAbility(abilityId);

            gm.SetAbilityEquipped(abilityId, !equipped);

            Debug.Log(!equipped ? "능력 장착!" : "능력 해제!");

            RefreshShop();
            return;
        }

        // 코인 차감
        if (!gm.SpendCoins(price))
        {
            Debug.Log("코인이 부족합니다!");
            return;
        }

        // 구매 성공
        gm.AddAbility(abilityId);

        // 구매와 동시에 장착
        gm.SetAbilityEquipped(abilityId, true);

        Debug.Log("능력 구매 완료! ID: " + abilityId);

        RefreshShop();
    }

    // 상점 UI 새로고침
    public void RefreshShop()
    {
        if (shopPanel == null)
            return;

        // ShopManager가 아니라 실제 상점 패널에서 검색
        AbilityItemUI[] items =
            shopPanel.GetComponentsInChildren<AbilityItemUI>(true);

        foreach (AbilityItemUI item in items)
        {
            item.UpdateItemUI();
        }
    }
}
