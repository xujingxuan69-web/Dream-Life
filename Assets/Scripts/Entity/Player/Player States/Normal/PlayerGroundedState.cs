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
    }

    public override void Exit()
    {
        base.Exit();
        player.jumpExtra = false;
        player.SetZeroVelocity();
    }

    public override void Update()
    {
        base.Update();

        

        if (!player.isBusy)
        {
            if (Input.GetButtonDown("Jump"))
            {
                stateMachine.ChangeState(player.jumpState);
                return;
            }
        }

        if (!player.IsGroundDetected())
        {
            player.dashExtra = true;
            player.SetJumpTimer();
            stateMachine.ChangeState(player.airState);
        }
    }
}
