using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearsAttackState : PlayerState
{
    public PlayerTearsAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.playerFx.StartTearsAttack();
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
            player.playerFx.StopTearsAttack();
            stateMachine.ChangeState(player.idleState);
        }
    }
}
