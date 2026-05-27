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
            default:
                stackSizeMax = 99;
                break;
        }   
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
