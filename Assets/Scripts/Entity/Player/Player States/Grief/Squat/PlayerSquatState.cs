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

        if (!player.IsHeadDetected())
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                stateMachine.ChangeState(player.squatExitState);
                return;
            }

            if (Input.GetKeyDown(KeyCode.L) && player.skill.dash.CanUseSkill() && (player.IsGroundDetected() || player.manager.dashExtra))
            {
                stateMachine.ChangeState(player.dashState);
                return;
            }
        }
        
    }
}
