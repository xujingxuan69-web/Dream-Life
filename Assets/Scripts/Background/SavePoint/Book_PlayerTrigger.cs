using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Book_PlayerTrigger : PlayerTrigger
{
    [SerializeField] private UI ui;

    protected override void Update()
    {
        if ((Input.GetKeyDown(KeyCode.F) && isTrigger && !ui.isMenuActive()) || (Input.GetMouseButtonDown(1) && ui.isMenuActive()))
        {
            ui.SwitchWithKey();
        }
    }
}
