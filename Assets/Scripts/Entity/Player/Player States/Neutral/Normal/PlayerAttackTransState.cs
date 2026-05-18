using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackTransState : PlayerState
{
    public PlayerAttackTransState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        stateMachine.ChangeState(player.primaryAttackState);
    }
}
