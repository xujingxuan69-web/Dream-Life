using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_UI : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private Transform formItemSlotParent;      //All the slots InChildren of inventorySlotParent
    [SerializeField] private Transform equipmentSlotParent;

    private ItemSlot_UI[] formItemSlot;     //Update itemSlot UI
    private ItemSlot_UI[] equipmentSlot;

    private void Start()
    {
        
    }
}