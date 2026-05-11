using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShowState : PlayerState
{
    public PlayerShowState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

        PlayerManager.instance.canDash = true;
        rb.constraints = RigidbodyConstraints2D.None;
        player.GetComponent<Collider2D>().enabled = true;
    }

    public override void Update()
    {
        base.Update();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
