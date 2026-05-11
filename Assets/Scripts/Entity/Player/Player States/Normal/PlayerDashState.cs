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
        player.playerFx.StopTearsAttack();

        player.skill.clone.CreateCloneOnDashStart();
            
        rb.gravityScale = 0;

        stateTimer = player.dashDuration;

        player.jumpExtra = player.IsGroundDetected() || player.jumpExtra;

        player.normalCollider.enabled = true;
    }

    public override void Exit()
    {
        base.Exit();

        rb.gravityScale = player.gravity;

        player.SetVelocity(0, 0);

        player.dashExtra = false;
    }

    public override void Update()
    {
        base.Update();

        if(!player.IsHeadDeatected() && !player.IsWallDetected())
        {
            player.SetVelocity(player.dashSpeed * player.dashDir, 0);
        }
        

        if(player.IsWallDetected() && !player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
            return;
        }

        if (Input.GetButtonDown("Jump") && player.jumpExtra)
        {
            stateMachine.ChangeState(player.jumpState);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.IsGroundDetected() ? player.idleState : player.airState);
        }
    }
}
