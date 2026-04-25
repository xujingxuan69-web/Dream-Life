using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleFaceStunState : EnemyState
{
    private Enemy_DoubleFace enemy;
    public DoubleFaceStunState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string animBoolName, Enemy_DoubleFace _enemy) : base(_enemyBase, _stateMachine, animBoolName)
    {
        this.enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.fx.Invoke("ShowStunVortex", 0);


        stateTimer = enemy.stunDuration;

        rb.velocity = new Vector2(-enemy.facingDir * enemy.stunDirection.sqrMagnitude, enemy.stunDirection.y);
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("DestoryStunVortex", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.stunIdleState);
        }
    }
}
