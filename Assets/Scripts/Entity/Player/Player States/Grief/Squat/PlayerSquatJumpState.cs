using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatJumpState : PlayerSquatState
{
    public PlayerSquatJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        rb.velocity = new Vector2(xInput * player.tempMoveSpeed, player.jumpForce);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.tempMoveSpeed, rb.velocity.y);

        if (rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(player.squatAirState);
        }
    }
}
