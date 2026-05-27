using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FxAnimationTrigger : MonoBehaviour
{
    protected Player player;

    protected virtual void Start()
    {
    }

    protected virtual void AnimationTriggerStart()
    {
        player = PlayerManager.instance.player; //!不得已使用
    }

    protected virtual void AnimationTriggerStop()
    {
        
    }
}
