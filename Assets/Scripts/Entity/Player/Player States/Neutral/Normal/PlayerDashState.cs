using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        rb.gravityScale = 0;
        stateTimer = player.dashDuration;

        player.manager.dashExtra = false;
        player.manager.jumpExtra = player.IsGroundDetected() || player.manager.jumpExtra;

        player.normalCollider.enabled = true;   //取消Squat状态的Collider限制
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = player.gravity;

        player.SetVelocity(0, 0);
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsHeadDetected() && !player.IsWallDetected())
        {
            player.SetVelocity(player.dashSpeed * player.facingDir, 0);
        }
        

        if(player.IsWallSlideDetected() && !player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        if (Input.GetButtonDown("Jump") && player.manager.jumpExtra)
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.IsGroundDetected() ? player.idleState : player.airState);
        }
    }
}
