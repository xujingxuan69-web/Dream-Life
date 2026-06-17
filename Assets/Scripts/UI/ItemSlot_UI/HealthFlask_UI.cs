using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HealthFlask_UI : ItemSlot_UI   
{
    protected override void Update() //由于血瓶属于永久性物品，所以无法被丢弃，在这里通过重写取消丢弃行为
    {
        
    }   

    public override void OnPointerDown(PointerEventData eventData)
    {
        
    }

}
