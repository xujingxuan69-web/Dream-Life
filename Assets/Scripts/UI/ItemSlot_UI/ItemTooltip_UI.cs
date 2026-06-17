using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip_UI : MonoBehaviour
{
    [Header("Basic Info")]
    [SerializeField] private GameObject equipTip;
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;

    [Header("Equipment")]
    [SerializeField] private Text itemType;
    [SerializeField] private Text itemStat;
    [SerializeField] private Text itemStatValue;
    [SerializeField] private Text itemDescription;
    [SerializeField] private List<GameObject> effectPanels;

    [Header("FormItem")]
    [SerializeField] private GameObject usablePanel_FormItem;
    [SerializeField] private Text itemUsableAmount_FormItem;
    [SerializeField] private Text itemStackSize_FormItem;

    [Header("UsableItem")]
    [SerializeField] private GameObject usablePanel_UsableItem;
    [SerializeField] private Text itemStackSize_UsableItem;
    [SerializeField] private Text itemStackSizeMax_UsableItem;


    private List<EffectSlot_UI>[] effectSlotLists;

    private void Awake()
    {
        effectSlotLists = new List<EffectSlot_UI>[effectPanels.Count];
        for (int i = 0; i < effectPanels.Count; i++)
        {
            var slots = effectPanels[i].GetComponentsInChildren<EffectSlot_UI>();
            effectSlotLists[i] = new List<EffectSlot_UI>(slots);
        }
    }

    private void Start()
    {
        SetDefaultPanel();
        HideToolTip();
    }

    #region ShowToolTip
    public void ShowToolTip(ItemData _item)
    {
        SetDefaultPanel();

        if (_item == null || string.IsNullOrEmpty(_item.name))
        {
            ShowEmptyEffectState();
            gameObject.SetActive(true);
            return;
        }

        itemIcon.sprite = _item.icon;
        itemName.text = _item.itemName;
        itemIcon.enabled = true;

        switch (_item.itemType)
        {
            case ItemType.Equipment:
                ShowEquipmentTooltip(_item as ItemData_Equipment);
                break;
            case ItemType.FormItem:
                ShowFormItemTooltip(_item);
                break;
            case ItemType.UsableItem:
                ShowUsableItemTooltip(_item);
                break;
        }

        gameObject.SetActive(true);
    }

    private void ShowEquipmentTooltip(ItemData_Equipment equipment)
    {
        if (equipment == null) return;

        itemType.text = GetEquipmentType(equipment.equipmentType);
        itemStat.text = PlayerManager.instance.player.stats.StatNameOfType(equipment.equipmentType);
        itemStatValue.text = GetStatValue(equipment).ToString();

        ShowEffects(equipment.itemEffects);
    }


    private void ShowFormItemTooltip(ItemData _item)
    {
        itemDescription.text = _item.description;
        if (Inventory.instance.formItemDictionary.TryGetValue(_item, out InventoryItem value))
        {
            itemUsableAmount_FormItem.text = $"{value.usableAmount}/{value.stackSize}";
            itemStackSize_FormItem.text = value.stackSize.ToString();
        }

        usablePanel_FormItem.SetActive(true);
    }

    private void ShowUsableItemTooltip(ItemData _item)
    {
        itemDescription.text = _item.description;
        usablePanel_UsableItem.SetActive(true);
    }
    #endregion
    #region Equipment
    private string GetEquipmentType(EquipmentType equipmentType)
    {
        return equipmentType switch
        {
            EquipmentType.Gaunlet => "È­Ì×",
            EquipmentType.Amulet => "ÏîÁ´",
            EquipmentType.Boots => "Ð¬×Ó",
            EquipmentType.Helmet => "Í·¿ø",
            _ => null
        };
    }

    private int GetStatValue(ItemData_Equipment equipment)
    {
        return equipment.equipmentType switch
        {
            EquipmentType.Gaunlet => equipment.strength,
            EquipmentType.Amulet => equipment.intelligence,
            EquipmentType.Boots => equipment.agility,
            EquipmentType.Helmet => equipment.vitality,
            _ => 0
        };
    }

    private void ShowEffects(List<ItemEffect> effects)
    {
        if (effects == null || effects.Count == 0)
        {
            ShowEmptyEffectState();
            return;
        }

        int effectCount = Mathf.Min(effects.Count, effectPanels.Count);

        for (int i = 0; i < effectCount; i++)
        {
            var panel = effectPanels[i];
            var slots = effectSlotLists[i];

            if (slots == null || slots.Count == 0) continue;

            var equipEffect = effects[i] as ItemEffect_Equipment;
            if (equipEffect != null)
            {
                slots[0].ShowSlotImage();
                slots[0].effectImage.sprite = equipEffect.effectIcon;
                slots[0].effectDescription.text = equipEffect.GetEffectDescription();
            }

            panel.SetActive(true);
        }
    }
    
    private void ShowEmptyEffectState()
    {
        if (effectSlotLists.Length > 0 && effectSlotLists[0].Count > 0)
        {
            effectSlotLists[0][0].HideSlotImage();
            effectSlotLists[0][0].effectDescription.text = "¡ª";
            effectPanels[0].SetActive(true);
        }
    }
    #endregion

    public void HideToolTip() => gameObject.SetActive(false);

    private void SetDefaultPanel()
    {
        itemIcon.enabled = false;
        itemName.text = null;
        itemType.text = null;

        itemStat.text = null;
        itemStatValue.text = null;
        itemDescription.text = null;
        equipTip.SetActive(false);
        usablePanel_FormItem.SetActive(false);
        usablePanel_UsableItem.SetActive(false);

        foreach (var panel in effectPanels)
            panel.SetActive(false);
    }
}