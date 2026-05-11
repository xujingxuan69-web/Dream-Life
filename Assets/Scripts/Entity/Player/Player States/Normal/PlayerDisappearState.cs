using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisappearState : PlayerState
{
    public PlayerDisappearState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        PlayerManager.instance.canDash = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Collider2D>().enabled = false;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }
}
