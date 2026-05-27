using UnityEngine;

public enum ItemType
{
    FormItem,
    Equipment,
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject    //In Unity Asset,Create Item with data visually
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
}
