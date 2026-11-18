
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AbilityItemUI : MonoBehaviour
{
    [Header("능력 데이터")]
    public AbilityData abilityData;

    [Header("상점")]
    public ShopManager shopManager;

    [Header("UI")]
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text priceText;

    public Button buyButton;

    // 버튼 내부에 표시되는 글자
    private TMP_Text buttonText;

    private void Start()
    {
        // 능력 정보 표시
        if (abilityData == null)
            return;

        if (nameText != null)
            nameText.text = abilityData.abilityName;

        if (descriptionText != null)
            descriptionText.text = abilityData.description;

        if (icon != null)
            icon.sprite = abilityData.icon;

        // 버튼 내부 TMP 텍스트 찾기
        if (buyButton != null)
        {
            buttonText = buyButton.GetComponentInChildren<TMP_Text>(true);

            buyButton.onClick.AddListener(BuyButtonClicked);
        }

        UpdateItemUI();
    }

    private void OnDestroy()
    {
        if (buyButton != null)
            buyButton.onClick.RemoveListener(BuyButtonClicked);
    }

    // 구매 버튼 클릭
    public void BuyButtonClicked()
    {
        if (shopManager == null || abilityData == null)
            return;

        shopManager.BuyOrEquipAbility(
            abilityData.abilityId,
            abilityData.price
        );
    }

    // UI 상태 갱신
    public void UpdateItemUI()
    {
        GameManager gm = GameManager.Instance;

        if (gm == null || abilityData == null)
            return;

        bool purchased = gm.HasAbility(abilityData.abilityId);
        bool equipped = gm.HasEquippedAbility(abilityData.abilityId);

        string displayText;

        if (equipped)
        {
            displayText = "장착중";
        }
        else if (purchased)
        {
            displayText = "장착하기";
        }
        else
        {
            displayText = abilityData.price.ToString("N0");
        }

        // 버튼 내부 글자 변경
        if (buttonText != null)
        {
            buttonText.text = displayText;
        }

        // 기존 가격 텍스트도 연결되어 있다면 갱신
        if (priceText != null)
        {
            priceText.text = displayText;
        }

        // 버튼 활성화
        if (buyButton != null)
        {
            buyButton.interactable = true;
        }
    }
}
