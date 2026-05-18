using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.manager.dashExtra = player.IsGroundDetected() || player.manager.dashExtra;
    }

    public override void Exit()
    {
        base.Exit();
        player.manager.jumpExtra = false;
    }

    public override void Update()
    {
        base.Update();
        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsWallSlideDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetButtonDown("Jump") && (player.manager.jumpExtra || player.jumpAirTimer > 0))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        #region  JumpState and AirState Share
        if (Input.GetKeyDown(KeyCode.L) && player.manager.dashExtra && player.skill.dash.CanUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.squatEnterState);
        }

        if (Input.GetKeyDown(KeyCode.N) && player.skill.blackhole.CanUseSkill())
        {
            stateMachine.ChangeState(player.disappearState);
        }
        #endregion
    }
}
