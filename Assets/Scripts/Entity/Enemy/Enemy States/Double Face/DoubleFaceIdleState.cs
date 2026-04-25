using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceIdleState : DoubleFaceGroundedState
{
    public DoubleFaceIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();

        enemy.SetZeroVelocity();
        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (rb.velocity.x != 0 && !enemy.isKnocked)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
            if (rb.velocity.x < 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (enemy.IsGroundDetected() || enemy.IsGroundBehindDetected() || enemy.IsGroundFrontDetected())
        {
            if (stateTimer < 0)
            {
                stateMachine.ChangeState(enemy.moveState);
            }
        }
    }
}
