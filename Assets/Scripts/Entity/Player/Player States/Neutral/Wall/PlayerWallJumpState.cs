using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.wallJumpDuration;
        player.SetVelocity(5 * player.facingDir, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
        player.manager.jumpExtra = false;
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.L) && player.manager.dashExtra && player.skill.dash.CanUseSkill())
        {
            player.skill.dash.UseSkill();
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (player.IsWallSlideDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if(player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
