using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatGroundedState : PlayerSquatState
{
    public PlayerSquatGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.manager.dashExtra = true;
        player.tempMoveSpeed = player.moveSpeed * player.moveSpeedRate;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetButtonDown("Jump") && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.squatJumpState);
            return;
        }

        if (!player.IsGroundDetected())
        {
            player.SetJumpAirTimer();
            stateMachine.ChangeState(player.squatAirState);
        }
    }
}
