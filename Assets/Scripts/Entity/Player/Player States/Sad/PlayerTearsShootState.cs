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
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            player.playerFx.StopTearsGenerate();
            stateMachine.ChangeState(player.idleState);
            return;
        }
    }
}
