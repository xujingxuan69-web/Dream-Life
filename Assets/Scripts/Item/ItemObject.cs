using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    private SpriteRenderer sr;
    [SerializeField] private ItemData itemData;

    private bool isPicked = false;  //防止多次触发

    private void OnValidate()   //OnValidate的内容将只在编辑器中调用 方便查看对应的Item
    {
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object - " + itemData.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPicked)
            return;

        if (collision.GetComponent<Player>() != null)
        {
            isPicked = true;
            if (itemData.itemType == ItemType.FormItem)
            {
                Inventory.instance.AddFormItem(itemData);
            }
            else if (itemData.itemType == ItemType.Equipment)
            {
                ItemData_equipment equipmentData = itemData as ItemData_equipment;  //提前进行类型转换
                Inventory.instance.AddEquipment(equipmentData);
            }
            
            Destroy(gameObject);
        }
    }
}
