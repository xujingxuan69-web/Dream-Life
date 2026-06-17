using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    HealthFlask,
    FormItem,
    Equipment,
    UsableItem,
    InventoryItem
}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject    //In Unity Asset,Create Item with data visually
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public string description;

    [Range(0, 100)]
    public float dropChance;

    public float itemCooldown;

    public List<ItemEffect> itemEffects = new List<ItemEffect>();

    public void ExecuteItemEffect()
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect();
        }
    }

    public void StopItemEffect()
    {
        foreach (var effect in itemEffects)
        {
            effect.StopEffect();
        }
    }

    public virtual void AddItemEffect()
    {
        ExecuteItemEffect();
    }

    public virtual void RemoveItemEffect()
    {
        StopItemEffect();
    }
}
