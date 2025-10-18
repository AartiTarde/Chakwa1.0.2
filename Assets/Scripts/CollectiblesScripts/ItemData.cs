using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "ScrollView/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public string description;

    public string buttonAText;
    public string buttonBText;
}
