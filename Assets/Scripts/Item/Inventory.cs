using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    [Header("FormItem")]
    public List<InventoryItem> formItems;  //Get item stacksize
    public Dictionary<ItemData, InventoryItem> formItemDictionary; //ItemData related to InventoryItem stacksize
    public Dictionary<int, InventoryItem> formItemIndexDictionary;
    private ItemSlot_UI[] formItemSlot;     //Update itemSlot UI
    [SerializeField] private Transform formItemSlotParent;      //In IventoryPanel,All the slots InChildren of inventorySlotParent

    //!formItem要添加与equipment一样的按位置添加、替换和删除，替换的formItem会继承原有formItem的stackSize，删除后会直接销毁，不会丢弃出东西

    [Header("Equipment")]
    public List<InventoryItem> equipment;
    public Dictionary<int, InventoryItem> equipmentIndexDictionary;
    private ItemSlot_UI[] equipmentSlot;
    [SerializeField] private Transform equipmentSlotParent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        formItems = new List<InventoryItem>();
        formItemDictionary = new Dictionary<ItemData, InventoryItem>();
        formItemIndexDictionary = new Dictionary<int, InventoryItem>();
        formItemSlot = formItemSlotParent?.GetComponentsInChildren<ItemSlot_UI>();

        equipment = new List<InventoryItem>();
        equipmentIndexDictionary = new Dictionary<int, InventoryItem>();
        equipmentSlot = equipmentSlotParent?.GetComponentsInChildren<ItemSlot_UI>();
    }

    private void UpdateSlotUI(ItemType _itemType)
    {
        if (_itemType == ItemType.FormItem)
        {
            for (int i = 0; i < formItemSlot.Length; i++)
            {
                InventoryItem item = formItemIndexDictionary.ContainsKey(i) ? formItemIndexDictionary[i] : null;
                formItemSlot[i].UpdateSlot(item);
            }
        }
        else if (_itemType == ItemType.Equipment)
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
            {
                InventoryItem item = equipmentIndexDictionary.ContainsKey(i) ? equipmentIndexDictionary[i] : null;
                equipmentSlot[i].UpdateSlot(item);
            }
        }
    }
    #region AddToFormItem
    private int GetSpareFormItemSlot() //获取空闲槽位，否则返回-1
    {
        for (int i = 0; i < formItemSlot.Length; i++)
        {
            if (formItemSlot[i].item == null)
                return i;
        }
        return -1;
    }

    public void AddFormItem(ItemData _item) //尝试添加到空闲槽位，如果不存在空闲槽位，打开装备界面
    {
        if (formItemDictionary.TryGetValue(_item, out InventoryItem value))    //Get InventoryItem value related to _item
        {
            value.AddStack();
        }
        else  //no _item in inventoryDictionary
        {
            int spareFormItemSlotIndex = GetSpareFormItemSlot();
            if (spareFormItemSlotIndex != -1)
            {
                InventoryItem newItem = new InventoryItem(_item);
                formItems.Add(newItem);
                formItemDictionary.Add(_item, newItem);
                formItemIndexDictionary.Add(spareFormItemSlotIndex, newItem);
            }
            else
            {
                //!打开面板手动选择索引替换
            }
                
        }

        UpdateSlotUI(ItemType.FormItem);
    }

    public void ReplaceFormItemAt(int _slotIndex, ItemData _newItem)    //手动选择槽位索引进行替换
    {
        /*InventoryItem inventoryItem = new InventoryItem(_newItem);
        if (_slotIndex < 0 || formItemSlot.Length <= _slotIndex)
        {
            Debug.Log("Out of formItem maxsize");
            return;
        }
        else if (formItemSlot[_slotIndex].item == null)
        {
            formItems.Add(inventoryItem);
            formItemDictionary.Add(_newItem, inventoryItem);
        }
        else
        {
            //!替换事件
        }

        UpdateSlotUI(ItemType.FormItem);*/
    }
    #endregion
    #region RemoveFromFormItem
    private void RemoveFormItemAt(int _slotIndex)   //手动丢弃FormItem
    {
        if (formItemIndexDictionary.TryGetValue(_slotIndex, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                formItems.Remove(value);
                formItemIndexDictionary.Remove(_slotIndex);
            }
            else
            {
                value.RemoveStack();
            }
        }
        else
        {
            Debug.Log("Warning: Wrong Remove formItem");
        }

        UpdateSlotUI(ItemType.FormItem);
    }
    #endregion

    #region AddToEquipment
    public int GetSpareEquipmentSlot() 
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            if (equipmentSlot[i].item == null)
                return i;
        }
        return -1;
    }

    public void AddEquipment(ItemData_equipment _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        int spareEquipmentSlotIndex = GetSpareEquipmentSlot();
        if (spareEquipmentSlotIndex == -1)
        {
            Debug.Log("No spareSlot");
            //!打开装备界面
            return;
        }

        equipment.Add(newItem);
        equipmentIndexDictionary.Add(spareEquipmentSlotIndex, newItem);
        _item.AddModifiers();

        UpdateSlotUI(ItemType.Equipment);
    }

    public void AddEquipmentAt(int _slotIndex, ItemData_equipment _newItem)   //equipment选定Slot添加/替换
    {
        InventoryItem newItem = new InventoryItem(_newItem);
        if (_slotIndex < 0)
        {
            Debug.Log("Warning: Wrong slotIndex");
            return;
        }
        else if (equipmentSlot[_slotIndex].item == null)
        {
            equipment.Add(newItem);
            equipmentIndexDictionary.Add(_slotIndex, newItem);
            _newItem.AddModifiers();
        }
        else
        {
            //!替换
        }

        UpdateSlotUI(ItemType.Equipment);
    }
    #endregion
    #region RemoveFromEquipment
    private void RemoveEquipmemtAt(int _slotIndex)
    {
        if (equipmentIndexDictionary.TryGetValue(_slotIndex, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                equipment.Remove(value);
                equipmentIndexDictionary.Remove(_slotIndex);
                ItemData_equipment equipItem = value.data as ItemData_equipment;
                equipItem.RemoveModifiers();
            }
            else
            {
                Debug.Log("Warning: Equipment stacksize out of stackSizeMax");
                value.RemoveStack();
            }
        }
        else
        {
            Debug.Log("Warning: Wrong Remove Equipment");
        }
        UpdateSlotUI(value.data.itemType);
    }
    #endregion
}
