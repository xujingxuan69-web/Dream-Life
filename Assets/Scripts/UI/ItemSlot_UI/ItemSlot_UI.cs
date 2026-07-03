using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler//引入鼠标接口
{
    [SerializeField] protected Image itemImage;
    [SerializeField] protected TextMeshProUGUI itemText;
    [SerializeField] protected Image selectionOutline;
    [SerializeField] protected Sprite originalSprite;
    [SerializeField] protected UnityEngine.Color originalColor;

    [SerializeField] protected List<ItemEffect> effectList = new List<ItemEffect>();  //!显示装备的词条

    public InventoryItem item;
    protected UnityEngine.Color color = UnityEngine.Color.white;

    public int slotIndex;    //用于定位当前槽位的索引，方便inventory进行操作
    protected bool isChosen;    //用于判定当前槽位是否被选中


    protected Menu_UI menu_ui;

    protected virtual void Awake()
    {
        menu_ui = UIManager.Instance.menu;
    }

    protected virtual void Update()
    {
        if (isChosen && Input.GetKeyDown(KeyCode.Alpha2))
        {
            DropItem();
        }
    }

    public void UpdateSlot(InventoryItem _newItem)  //Update itemSlot UI -- image + stacksize,and index
    {

        if (_newItem == null)
        {
            Debug.Log(Time.time+" Trigger with UpdateSlot Null");
        }

        item = _newItem;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;
            itemImage.color = color;
            SetText();

            effectList = item.data.itemEffects;
        }
        else
        {
            itemImage.sprite = originalSprite;
            itemImage.color = originalColor;
            itemText.text = null;
        }
    }

    private void SetText()
    {
        if (item.stackSize > 1)
            itemText.text = item.stackSize.ToString();
        else
            itemText.text = "";
    }

    public void SetDefaultIndex(int _slotIndex) => slotIndex = _slotIndex;

    public void DropItem()
    {
        if (item == null || item?.data == null) 
        {
            Debug.Log("empty item data");
            return;
        }
        Player player = PlayerManager.instance.player;
        ItemData newItem = item.data;
        Inventory.instance.RemoveItemAt(slotIndex, newItem.itemType);
        player.itemDrop.DropItem(newItem, player.facingDir, player.IsGroundDetected());

        menu_ui.itemToolTip.ShowToolTip(item?.data);
    }

    #region OnPointerEvents
    public void SetChosen(bool _set) => isChosen = _set;    //给Inventory_UI调用，进行Chosen设置

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        DropItem();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        isChosen = true;
        //!调用Inventory_UI的instance，表示当前FormItemSlot/equipmentSlot被选中，让其记录下当前FormItemSlot/equipmentSlot的index

        if (menu_ui == null)
            Debug.Log("UI null");
        else if (menu_ui.itemToolTip == null)
            Debug.Log("UI itemToolTip null");

        menu_ui.itemToolTip.ShowToolTip(item?.data);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        isChosen = false;
        menu_ui.itemToolTip.HideToolTip();
    }

    public virtual void OnPointerMove(PointerEventData eventData)
    {
    }
    #endregion
}
