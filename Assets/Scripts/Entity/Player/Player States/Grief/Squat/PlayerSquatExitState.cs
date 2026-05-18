using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatExitState : PlayerState
{
    public PlayerSquatExitState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.normalCollider.enabled = true;

        stateTimer = 0.3f;
        
        if (player.IsGroundDetected())
        {
            player.SetZeroVelocity();
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.tempMoveSpeed = player.moveSpeed;
        player.squatEnter = false;
    }

    public override void Update()
    {
        base.Update();

        if (!player.IsGroundDetected())
        {
            player.SetVelocity(xInput * player.tempMoveSpeed, rb.velocity.y);
        }
        else if (rb.velocity.x != 0 && !player.isKnocked)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        if (stateTimer < 0)
        {
            player.normalCollider.enabled = true;
            if (player.IsGroundDetected())

                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState);
        }
    }
}
