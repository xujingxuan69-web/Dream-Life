using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceStunIdleState : EnemyState
{
    private Enemy_DoubleFace enemy;
    public DoubleFaceStunIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunIdleDuration;
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
            if(enemy.IsGroundBehindDetected() || enemy.IsGroundDetected() || enemy.IsGroundFrontDetected())
            {
                stateMachine.ChangeState(enemy.moveState);
            }
            else
            {
                stateMachine.ChangeState(enemy.idleState);
            }
            
        }
    }
}
