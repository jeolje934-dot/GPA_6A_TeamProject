
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewAbility",
    menuName = "Game/Ability Data"
)]
public class AbilityData : ScriptableObject
{
    [Header("능력 정보")]
    public int abilityId;
    public string abilityName;
    [TextArea]
    public string description;

    [Header("가격")]
    public int price;

    [Header("아이콘")]
    public Sprite icon;
}
