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
        player.jumpExtra = false;
        player.dashExtra = player.IsGroundDetected() || player.dashExtra;

        rb.velocity = new Vector2(xInput * player.moveSpeed, player.jumpForce);
        stateTimer = 0.2f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (player.IsWallDetected() && stateTimer < 0)
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (rb.velocity.y <= 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.squatEnterState);
        }
    }
}
