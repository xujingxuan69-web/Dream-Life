using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_UI : UI
{
    [SerializeField] private GameObject equipmentUI;
    [SerializeField] private GameObject stateUI;
    [SerializeField] private GameObject inventoryUI;
    [SerializeField] private GameObject settingUI;

    public ItemTooltip_UI itemToolTip;
    public InventoryItemTooltip_UI inventoryItemToolTip;

    protected override void Awake()
    {
        base.Awake();
        defaultPanel = equipmentUI;
        panelList.Add(equipmentUI);
        panelList.Add(stateUI);
        panelList.Add(inventoryUI);
        panelList.Add(settingUI);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetMouseButtonDown(1) && isMenuActive()))
            SwitchWithKey();
    }
}
