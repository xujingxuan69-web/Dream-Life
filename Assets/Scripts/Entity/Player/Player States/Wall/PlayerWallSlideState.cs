using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
        player.Flip();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Jump"))
        {
            player.dashExtra = true;
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }//如果不加return的话，下面的内容在更新前也会一起执行，导致跳跃的横向移动变为0，或者我们可以把该语句放到最底下，最保险的方式仍然是加上return语句

        if (yInput < 0)
        {
            player.SetVelocity(0, rb.velocity.y);
        }
        else
        {
            player.SetVelocity(0, rb.velocity.y * 0.5f);
        }

        if (!player.IsWallDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
