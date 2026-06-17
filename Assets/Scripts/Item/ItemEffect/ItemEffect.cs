using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffect : ScriptableObject
{
    protected Player player;

    public virtual void ExecuteEffect()
    {
        player = PlayerManager.instance.player;
    }

    public virtual void StopEffect()
    {
    }
}
