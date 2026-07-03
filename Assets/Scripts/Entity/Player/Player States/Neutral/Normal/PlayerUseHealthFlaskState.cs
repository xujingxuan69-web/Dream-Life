using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseHealthFlaskState : PlayerState
{
    public PlayerUseHealthFlaskState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.5f;
        player.onHealthFlaskUsed?.Invoke();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }
}
