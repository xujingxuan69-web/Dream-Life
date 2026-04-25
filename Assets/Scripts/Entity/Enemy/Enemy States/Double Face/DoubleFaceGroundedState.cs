using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class DoubleFaceGroundedState : EnemyState
{
    protected Transform player;

    protected Enemy_DoubleFace enemy;
    public DoubleFaceGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName)
    {
        this.enemy = _enemy;
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
        
        if (enemy.IsGroundDetected() && enemy.IsGroundFrontDetected() && !enemy.IsWallDetected())
        {
            if (enemy.IsPlayerHit || Vector2.Distance(enemy.transform.position, player.position) < enemy.attackDistance && !enemy.IsWallDetected())
            {
                stateMachine.ChangeState(enemy.battleState);
            }
        }

    }
}
