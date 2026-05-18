using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxAnimationTrigger : MonoBehaviour
{
    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void AnimationTriggerStart()
    {
        
    }

    protected virtual void AnimationTriggerStop()
    {
        
    }
}
