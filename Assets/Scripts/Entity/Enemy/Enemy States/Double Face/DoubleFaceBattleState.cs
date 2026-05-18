using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceBattleState : EnemyState
{
    private Transform player;
    private Enemy_DoubleFace enemy;
    private int moveDir;
    public DoubleFaceBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName)
    {
        enemy = _enemy;

    }

    public override void Enter()
    {
        base.Enter();
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
            stateTimer = enemy.battleTime;
            if (enemy.hitFront.distance < enemy.attackDistance && CanAttack())
            {
                stateMachine.ChangeState(enemy.attackState);
            }

        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 50)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        if (enemy.IsWallDetected() || (!enemy.IsGroundFrontDetected() && enemy.IsGroundDetected()) 
            || (Mathf.Abs(player.transform.position.x - enemy.transform.position.x) < 0.5f * enemy.attackDistance && (enemy.IsPlayerFrontHit || enemy.IsPlayerBehindHit)))
        {
            stateMachine.ChangeState(enemy.battleIdleState);
            return;
        }

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.battleSpeed * moveDir, rb.velocity.y);
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
