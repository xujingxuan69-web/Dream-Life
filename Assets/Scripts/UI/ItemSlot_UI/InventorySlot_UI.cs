using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot_UI : ItemSlot_UI
{
    protected override void Update()
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        menu_ui.inventoryItemToolTip.ShowToolTip(item?.data);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        menu_ui.inventoryItemToolTip.ShowToolTip(null);
    }
}
