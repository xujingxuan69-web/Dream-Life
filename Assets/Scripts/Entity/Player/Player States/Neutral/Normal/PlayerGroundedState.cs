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

        player.SetJumpAirTime();

        if (!player.isBusy) //攻击结束后摇
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                stateMachine.ChangeState(player.useHealthFlaskState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.Q) && player.CheckCounterAttackTime())
            {
                stateMachine.ChangeState(player.counterAttackState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.J))
            {
                if (player.GetPlayerForm() != FormType.Grief)
                    stateMachine.ChangeState(player.primaryAttackState);
                else
                    stateMachine.ChangeState(player.tearsAimState);
                return;
            }

            if (Input.GetButtonDown("Jump") && player.CheckJumpAirTime())
            {
                player.manager.dashExtra = true;
                stateMachine.ChangeState(player.jumpState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(player.squatEnterState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.M))    //!demo暂时演示
            {
                stateMachine.ChangeState(player.tearsAimState);
            }

            if (Input.GetKeyDown(KeyCode.N) && player.skill.blackhole.CanUseSkill()) 
            {
                stateMachine.ChangeState(player.disappearState);
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.L) && player.skill.dash.CanUseSkill()) //冲刺取消后摇
        {
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            player.manager.dashExtra = true;
            player.SetJumpAirTime();
            stateMachine.ChangeState(player.airState);
        }
    }
}
