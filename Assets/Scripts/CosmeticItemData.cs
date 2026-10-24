
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCosmeticItem",
    menuName = "Game/Cosmetic Item"
)]
public class CosmeticItemData : ScriptableObject
{
    [Header("아이템 정보")]
    public int itemId;
    public string itemName;

    [Header("치장 모델")]
    public GameObject modelPrefab;

    [Header("인벤토리 아이콘")]
    public Sprite icon;
}
