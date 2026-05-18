using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearsShootState : PlayerState
{
    public PlayerTearsShootState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.playerFx.StartTearsGenerate();
    }

    public override void Exit()
    {
        base.Exit();
        player.playerFx.StopTearsGenerate();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.L) && player.skill.dash.CanUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
            return;
        }

        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
