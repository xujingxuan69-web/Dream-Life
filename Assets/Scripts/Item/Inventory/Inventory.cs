using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public Player player;

    #region Slot
    [Header("HealthFlask_UsableItem")]
    public List<InventoryItem> healthFlask;
    public List<ItemData> startingHealthFlask;
    public Dictionary<ItemData, InventoryItem> healthFlaskDictionary;
    public Dictionary<int, InventoryItem> healthFlaskIndexDictionary;

    [Header("FormItem_UsableItem")]
    public List<InventoryItem> formItems;  //Get item stacksize
    public List<ItemData> startingFormItem;
    public Dictionary<ItemData, InventoryItem> formItemDictionary; //ItemData related to InventoryItem stacksize
    public Dictionary<int, InventoryItem> formItemIndexDictionary;

    //!formItem要添加与equipment一样的按位置添加、替换和删除，替换的formItem会继承原有formItem的stackSize，删除后会直接销毁，不会丢弃出东西

    [Header("Equipment")]
    public List<InventoryItem> equipment;
    public List<ItemData_Equipment> startingEquipment;
    public Dictionary<int, InventoryItem> equipmentIndexDictionary;


    [Header("Inventory UI")]
    [SerializeField] private Transform healthFlaskSlotParent;
    [SerializeField] private Transform formItemSlotParent;      //In IventoryPanel,All the slots InChildren of inventorySlotParent
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private ItemSlot_UI[] healthFlaskSlot;
    private ItemSlot_UI[] formItemSlot;     //Update itemSlot UI
    private ItemSlot_UI[] equipmentSlot;
    private StatSlot_UI[] statSlot;
    
    #endregion

    [Header("Item Effect")]
    public List<ItemEffect_Equipment> possibleEquipmentEffects;   //通过Inentory分配可能存在的词条
    private Dictionary<EffectType, List<ItemEffect_Equipment>> effectTypeDictionary;


    [Header("Item Cooldown")]
    private float lastTimeUsedHealthFlask;
    private float flaskCooldown;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        healthFlask = new List<InventoryItem>();
        healthFlaskDictionary = new Dictionary<ItemData, InventoryItem>();
        healthFlaskIndexDictionary = new Dictionary<int, InventoryItem>();

        formItems = new List<InventoryItem>();
        formItemDictionary = new Dictionary<ItemData, InventoryItem>();
        formItemIndexDictionary = new Dictionary<int, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentIndexDictionary = new Dictionary<int, InventoryItem>();

        healthFlaskSlot = healthFlaskSlotParent?.GetComponentsInChildren<HealthFlask_UI>();
        formItemSlot = formItemSlotParent?.GetComponentsInChildren<FormItem_UI>();
        equipmentSlot = equipmentSlotParent?.GetComponentsInChildren<Equipment_UI>();
        statSlot = statSlotParent?.GetComponentsInChildren<StatSlot_UI>();

        effectTypeDictionary = new Dictionary<EffectType, List<ItemEffect_Equipment>>();

        player = PlayerManager.instance.player;
        player.stats.vitality.onValueChanged += UpdateStatSlotUI;
        player.stats.baseHealth.onValueChanged += UpdateStatSlotUI;
        player.stats.maxHealthRate.onValueChanged += UpdateStatSlotUI;
        player.stats.baseFormFocus.onValueChanged += UpdateStatSlotUI;
        player.stats.maxFormFocusRate.onValueChanged += UpdateStatSlotUI;

        AddStartingItems();
        SortEffectsType();
        SetSlotIndex();
    }

    #region SetDefault
    private void AddStartingItems()
    {
        for (int i = 0; i < startingHealthFlask.Count; i++)
        {
            AddHealthFlask(startingHealthFlask[i]);
        }
        for (int i = 0; i < startingFormItem.Count; i++)
        {
            AddFormItem(startingFormItem[i]);
        }
        for (int i = 0; i < startingEquipment.Count; i++)
        {
            AddEquipment(startingEquipment[i]);
        }
    }
    private void SortEffectsType()
    {
        for (int i = 0; i < possibleEquipmentEffects.Count; i++)
        {
            ItemEffect_Equipment effect = possibleEquipmentEffects[i];
            if (!effectTypeDictionary.ContainsKey(effect.effectType))
                effectTypeDictionary[effect.effectType] = new List<ItemEffect_Equipment>();
            effectTypeDictionary[effect.effectType].Add(effect);
        }
    }

    private void SetSlotIndex()
    {
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].SetDefaultIndex(i);
        }
        for (int i = 0; i < formItemSlot.Length; i++)
        {
            formItemSlot[i].SetDefaultIndex(i);
        }
        for (int i = 0; i < healthFlaskSlot.Length; i++)
        {
            healthFlaskSlot[i].SetDefaultIndex(i);
        }
    }
    #endregion
    #region Effects
    public List<EffectType> GetEffectTypes() => effectTypeDictionary.Keys.ToList();

    public ItemEffect_Equipment GetRandomEffect(EffectType _type)
    {
        List<ItemEffect_Equipment> effects = effectTypeDictionary[_type];
        return effects[Random.Range(0, effects.Count())];
    }
    #endregion

    #region Slot
    private void UpdateSlotUI(ItemType _itemType)
    {
        if (_itemType == ItemType.HealthFlask)
        {
            for (int i = 0; i < healthFlaskSlot.Length; i++)
            {
                InventoryItem item = healthFlaskIndexDictionary.ContainsKey(i) ? healthFlaskIndexDictionary[i] : null;
                healthFlaskSlot[i].UpdateSlot(item);
            }
        }
        else if (_itemType == ItemType.FormItem)
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

    private void UpdateStatSlotUI()
    {
        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }

    private int GetSpareSlotIndex(ItemData _item)
    {
        ItemType itemType = _item.itemType;
        ItemSlot_UI[] slot;
        switch (itemType)
        {
            case ItemType.HealthFlask:
                slot = healthFlaskSlot;
                break;
            case ItemType.FormItem:
                slot = formItemSlot;
                break;
            case ItemType.Equipment:
                slot = equipmentSlot;
                break;
            default:
                slot = null;
                break;
        }

        for (int i = 0; i < slot.Length; i++)
        {
            if (slot[i].item?.stackSize == 0 || slot[i].item == null)
                return i;
        }
        return -1;
    }

    public bool CanAddItem(ItemData _item) => GetSpareSlotIndex(_item) != -1;
    #endregion

    #region AddItem
    public void AddItem(ItemData _item)
    {
        ItemType itemtype = _item.itemType;
        switch (itemtype)
        {
            case ItemType.HealthFlask:
                AddHealthFlask(_item);
                break;
            case ItemType.FormItem:
                AddFormItem(_item); 
                break;
            case ItemType.Equipment:
                AddEquipment(_item);
                break;
        }
    }

    public void AddItemAt(int _slotIndex, ItemData_Equipment _item)
    {
        ItemType itemtype = _item.itemType;
        switch (itemtype)
        {
            case ItemType.Equipment:
                AddEquipmentAt(_slotIndex, _item);
                break;
        }
    }

    #region AddHealthFlask
    private void AddHealthFlask(ItemData _item)
    {
        if (healthFlaskDictionary.TryGetValue(_item, out InventoryItem value))    //Get InventoryItem value related to _item
        {
            value.AddStack();
        }
        else  //no _item in inventoryDictionary
        {
            int spareSlotIndex = GetSpareSlotIndex(_item);
            if (spareSlotIndex != -1)
            {
                InventoryItem newItem = new InventoryItem(_item);
                healthFlask.Add(newItem);
                healthFlaskDictionary.Add(_item, newItem);
                healthFlaskIndexDictionary.Add(spareSlotIndex, newItem);
                _item.AddItemEffect();
            }
            else
            {
                Debug.Log("Wrong Item Add to HealthFlask");
            }
        }

        UpdateSlotUI(ItemType.HealthFlask);
    }
    #endregion

    #region AddFormItem
    private void AddFormItem(ItemData _item) //尝试添加到空闲槽位，如果不存在空闲槽位，打开装备界面
    {
        if (formItemDictionary.TryGetValue(_item, out InventoryItem value))    //Get InventoryItem value related to _item
        {
            value.AddStack();
        }
        else  //no _item in inventoryDictionary
        {
            int spareSlotIndex = GetSpareSlotIndex(_item);
            if (spareSlotIndex != -1)
            {
                InventoryItem newItem = new InventoryItem(_item);
                formItems.Add(newItem);
                formItemDictionary.Add(_item, newItem);
                formItemIndexDictionary.Add(spareSlotIndex, newItem);
                _item.AddItemEffect();
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

    #region AddEquipment
    private void AddEquipment(ItemData _item)
    {
        InventoryItem newItem = new InventoryItem(_item);
        int spareSlotIndex = GetSpareSlotIndex(_item);
        if (spareSlotIndex == -1)
        {
            Debug.Log("No spareSlot");
            //!打开装备界面
            return;
        }

        equipment.Add(newItem);
        equipmentIndexDictionary.Add(spareSlotIndex, newItem);
        _item.AddItemEffect();

        UpdateSlotUI(ItemType.Equipment);
    }

    private void AddEquipmentAt(int _slotIndex, ItemData_Equipment _item)   //equipment选定Slot添加/替换
    {
        InventoryItem newItem = new InventoryItem(_item);
        if (_slotIndex < 0)
        {
            Debug.Log("Warning: Wrong slotIndex");
            return;
        }
        else if (equipmentSlot[_slotIndex].item == null)
        {
            equipment.Add(newItem);
            equipmentIndexDictionary.Add(_slotIndex, newItem);
            _item.AddItemEffect();
        }
        else
        {
            //!替换
        }

        UpdateSlotUI(ItemType.Equipment);
    }
    #endregion

    #endregion
    #region RemoveItem
    public void RemoveItemAt(int _slotIndex, ItemType _type)
    {
        switch (_type)
        {
            case ItemType.FormItem:
                RemoveFormItemAt(_slotIndex);
                break;
            case ItemType.Equipment:
                RemoveEquipmentAt(_slotIndex);
                break;
        }
    }

    #region RemoveFromFormItem
    private void RemoveFormItemAt(int _slotIndex)   //手动丢弃FormItem
    {
        if (formItemIndexDictionary.TryGetValue(_slotIndex, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                formItems.Remove(value);
                formItemDictionary.Remove(value.data);
                formItemIndexDictionary.Remove(_slotIndex);
                value.data.RemoveItemEffect();
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

    #region RemoveFromEquipment
    private void RemoveEquipmentAt(int _slotIndex)
    {
        if (equipmentIndexDictionary.TryGetValue(_slotIndex, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                equipment.Remove(value);
                equipmentIndexDictionary.Remove(_slotIndex);
                value.data.RemoveItemEffect();
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

    #endregion

    #region HealthFlask
    public ItemData GetHealthFlask(ItemType _type)
    {
        ItemData flaskItem = null;

        foreach (KeyValuePair<ItemData, InventoryItem> item in healthFlaskDictionary)
        {
            if (item.Key.itemType == ItemType.HealthFlask)
            {
                flaskItem = item.Key;
            }
        }

        return flaskItem;
    }

    public bool CheckHealthFlaskUsableAmount(ItemData _item)
    {
        if (healthFlaskDictionary.TryGetValue(_item, out InventoryItem _value))
        {
            return _value.CheckItemUsable();
        }
        return false;
    }

    public bool UseHealthFlask()
    {
        ItemData currentFlask = GetHealthFlask(ItemType.HealthFlask);

        if (currentFlask == null)
        {
            Debug.Log("GetHealthFlask false");
            return false;
        }
        
        bool canUseFlask = Time.time > lastTimeUsedHealthFlask + flaskCooldown;
        if (canUseFlask && CheckHealthFlaskUsableAmount(currentFlask))
        {
            flaskCooldown = currentFlask.itemCooldown;
            lastTimeUsedHealthFlask = Time.time;
            return true;
        }
        else
        {
            if (!canUseFlask)
                Debug.Log("HealthFlask Cooldown");
            if (!CheckHealthFlaskUsableAmount(currentFlask))
                Debug.Log("HealthFlask Usable Amount loss");

            return false;
        }
    }
    #endregion

    private void OnDestroy()
    {
        player.stats.vitality.onValueChanged -= UpdateStatSlotUI;
        player.stats.baseHealth.onValueChanged -= UpdateStatSlotUI;
        player.stats.maxHealthRate.onValueChanged -= UpdateStatSlotUI;
        player.stats.baseFormFocus.onValueChanged -= UpdateStatSlotUI;
        player.stats.maxFormFocusRate.onValueChanged -= UpdateStatSlotUI;
    }
}