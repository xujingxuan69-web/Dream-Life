using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.SetZeroVelocity();

        player.manager.jumpExtra = false;
        player.manager.dashExtra = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        player.SetJumpAirTimer();

        if (!player.isBusy) //攻击结束后摇
        {
            if (Input.GetButtonDown("Jump") && player.jumpAirTimer > 0)
            {
                player.manager.dashExtra = true;
                stateMachine.ChangeState(player.jumpState);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && player.skill.dash.CanUseSkill()) //冲刺取消后摇
        {
            player.skill.clone.CreateCloneOnDashStart();
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            player.manager.dashExtra = true;
            player.SetJumpAirTimer();
            stateMachine.ChangeState(player.airState);
        }
    }
}
