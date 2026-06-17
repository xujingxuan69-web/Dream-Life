using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventoryItem  //Just for stacksize
{
    public ItemData data;
    public int stackSizeMax;
    public int stackSize;
    public int usableAmount;

    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();

        switch (data.itemType)
        {
            case ItemType.FormItem:
                stackSizeMax = 3;
                break;
            case ItemType.Equipment:
                stackSizeMax = 1;
                break;
            case ItemType.UsableItem:
                stackSizeMax = 2;
                break;
            case ItemType.InventoryItem:
                stackSizeMax = 1;
                break;
            default:
                stackSizeMax = 99;
                break;
        }
    }

    public void AddStack()
    {
        stackSize++;
        switch (data.itemType)
        {
            case ItemType.HealthFlask:
                usableAmount = stackSize;
                break;
            case ItemType.FormItem:
                usableAmount = stackSize;
                break;
            default:
                usableAmount = 0;
                break;
        }
    }
    public void RemoveStack()
    {
        stackSize--;
        if(usableAmount > stackSize)
        {
            usableAmount = stackSize;
        }
    }

    public bool CheckItemUsable()
    {
        if (usableAmount > 0)
        {
            usableAmount--;
            return true;
        }
        return false;
    }
}
