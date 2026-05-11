using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatState : PlayerState
{
    public PlayerSquatState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (!player.IsHeadDeatected())
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(player.squatExitState);
                return;
            }
        }
        
    }
}
