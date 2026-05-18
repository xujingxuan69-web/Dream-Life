using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatAirState : PlayerSquatState
{
    public PlayerSquatAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.tempMoveSpeed, rb.velocity.y);

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.squatIdleState);
        }
            
        if (Input.GetButtonDown("Jump") && (player.jumpAirTimer > 0))
        {
            stateMachine.ChangeState(player.squatJumpState);
        }
    }
}
