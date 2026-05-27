using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot_UI : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler //引入鼠标接口
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private Image selectionOutline;

    public InventoryItem item;

    private UnityEngine.Color color = UnityEngine.Color.white;

    private void Start()
    {
        UpdateSlot(null);
    }

    public void UpdateSlot(InventoryItem _newItem)  //Update itemSlot UI -- image + stacksize
    {
        item = _newItem;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
                itemText.text = item.stackSize.ToString();
            else
                itemText.text = "";

            SetImage(true);
        }
        else
        {
            itemImage.sprite = null;
            itemText.text = null;
            SetImage(false);
        }
    }

    private void SetImage(bool _set)
    {
        color.a = _set ? 1:0;
        itemImage.color = color;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (item.data.itemType == ItemType.Equipment)
        {
            Debug.Log("Equipment new item + " + item?.data.itemName);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
