using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceMoveState : DoubleFaceGroundedState
{
    public DoubleFaceMoveState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if ((enemy.IsGroundDetected() && !enemy.IsGroundFrontDetected()) || enemy.IsWallDetected())
        {
            enemy.Flip();
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetVelocity(enemy.moveSpeed * enemy.facingDir, enemy.rb.velocity.y);

        if (enemy.IsWallDetected() || (!enemy.IsGroundFrontDetected() && enemy.IsGroundDetected()) || 
            (!enemy.IsGroundFrontDetected() && !enemy.IsGroundBehindDetected() && !enemy.IsGroundDetected())) //Åö±Ú||ÐüÑÂ||¸¡¿Õ
        {
            stateMachine.ChangeState(enemy.idleState);
        }

        
    }
}
