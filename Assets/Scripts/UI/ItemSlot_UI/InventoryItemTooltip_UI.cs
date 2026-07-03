using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemTooltip_UI : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private Text itemName;
    [SerializeField] private Text usage;
    [SerializeField] private Text detail;

    private void Start()
    {
        SetDefaultPanel();
    }

    public void ShowToolTip(ItemData _item)
    {
        SetDefaultPanel();

        if (_item == null || string.IsNullOrEmpty(_item.name))
        {
            return;
        }
        itemIcon.sprite = _item.icon;
        itemName.text = _item.itemName;
        usage.text = _item.usage;
        detail.text = _item.detail;

        itemIcon.enabled = true;
    }

    private void SetDefaultPanel()
    {
        itemIcon.enabled = false;
        itemName.text = null;
        usage.text = null;
        detail.text = null;
    }
}
