using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceBattleIdleState : EnemyState
{
    private Transform player;
    private Enemy_DoubleFace enemy;
    private int moveDir;
    public DoubleFaceBattleIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.idleTime;
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerFrontHit)
        {
            if (enemy.hitFront.distance < enemy.attackDistance && CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.moveState);
            return;
        }

        if (Vector2.Distance(player.transform.position, enemy.transform.position) > enemy.attackDistance  
            && Mathf.Abs(player.transform.position.x - enemy.transform.position.x) > 0.5f * enemy.attackDistance
            && enemy.IsGroundFrontDetected() && enemy.IsGroundDetected() && !enemy.IsWallDetected())
        {
            stateMachine.ChangeState(enemy.battleState);
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (moveDir != enemy.facingDir)
        {
            enemy.Flip();
        }
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            return true;
        }
        return false;
    }
}
