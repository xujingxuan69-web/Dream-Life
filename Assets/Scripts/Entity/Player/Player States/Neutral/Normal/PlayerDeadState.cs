using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.deadAirState);
            return;
        }
        else
        {
            player.CollidersFreeze(true);
            player.ConstraintsFreeze(true);
        }
    }

    public override void Exit()
    {
        base.Exit();

        player.CollidersFreeze(false);
        player.ConstraintsFreeze(false);
    }

    public override void Update()
    {
        base.Update();

        
    }
}
