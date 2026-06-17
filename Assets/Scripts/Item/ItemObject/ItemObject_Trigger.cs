using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject_Trigger : MonoBehaviour
{
    private ItemObject myItemObject => GetComponentInParent<ItemObject>();

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!myItemObject.canPicked())
            return;

        if (collision.GetComponent<Player>() != null)
        {
            if (PlayerManager.instance.player.stats.isDead) //”≈ªØGetComponent
                return;

            myItemObject.PickupItem();
        }
    }
}
