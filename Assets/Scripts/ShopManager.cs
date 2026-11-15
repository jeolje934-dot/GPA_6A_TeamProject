using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("상점 UI")]
    [SerializeField] private GameObject shopPanel;

    // 상점 열기
    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    // 상점 닫기
    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }
}