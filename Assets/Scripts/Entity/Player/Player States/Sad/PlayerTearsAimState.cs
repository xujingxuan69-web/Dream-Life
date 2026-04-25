using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearsAimState : PlayerState
{
    public PlayerTearsAimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.5f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.M))
        {
            if (stateTimer > 0)
            {
                stateMachine.ChangeState(player.tearsAttackState);
            }
            else
            {
                stateMachine.ChangeState(player.tearsShootState);
            }
        }
    }
}
