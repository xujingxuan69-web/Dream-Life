using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.2f;
        rb.velocity = new Vector2(xInput * player.moveSpeed, player.jumpForce);

        player.manager.jumpExtra = false;
        player.manager.dashExtra = player.IsGroundDetected() || player.manager.dashExtra;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsWallSlideDetected() && stateTimer < 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(player.airState);
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
