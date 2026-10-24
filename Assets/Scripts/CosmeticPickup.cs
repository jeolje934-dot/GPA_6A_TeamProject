
using UnityEngine;

public class CosmeticPickup : MonoBehaviour
{
    [Header("치장 아이템 데이터")]
    public CosmeticItemData itemData;

    private bool collected = false;

    public void Collect(GameManager manager)
    {
        if (collected || manager == null || itemData == null)
        {
            return;
        }

        // 이미 보유한 아이템이면 획득하지 않음
        if (manager.HasItem(itemData.itemId))
        {
            Debug.Log("이미 보유한 아이템입니다.");
            return;
        }

        // 획득 성공 시 저장
        if (manager.AddItem(itemData.itemId))
        {
            collected = true;

            Debug.Log("치장 아이템 획득: " + itemData.itemName);

            Destroy(gameObject);
        }
    }
}
