using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item Effect/HealthFlask/HealthFlaskExecute")]
public class HealthFlaskExecute : ItemEffect_Usable
{
    public override void ExecuteEffect()
    {
        base.ExecuteEffect();
        player.onHealthFlaskUsed += UseItemEffect;
    }

    public override void StopEffect()
    {
        base.StopEffect();
        player.onHealthFlaskUsed -= UseItemEffect;
    }

    public override void UseItemEffect()
    {
        if (Inventory.instance.UseHealthFlask())
        {
            int amount = Mathf.RoundToInt(player.stats.maxHealth * 0.6f);
            amount = Mathf.Clamp(amount, 1, int.MaxValue);
            player.stats.IncreaseHealthBy(amount);
        }
    }
}
