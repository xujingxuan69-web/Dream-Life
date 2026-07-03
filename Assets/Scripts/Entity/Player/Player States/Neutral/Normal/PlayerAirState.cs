using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirState : PlayerUngroundedState
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
        if (player.IsWallSlideDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetButtonDown("Jump") && (player.manager.jumpExtra || player.CheckJumpAirTime()))
        {
            stateMachine.ChangeState(player.jumpState);
        }

        base.Update();
    }
}
