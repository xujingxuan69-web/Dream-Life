using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DoubleFace : Enemy
{
    [Header("Attack Info")]
    public float battleSpeed;
    public float battleTime;
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector] public float lastTimeAttacked;


    


    public RaycastHit2D hit { get; private set; }
    public bool IsPlayerHit { get; private set; }
    #region States
    public DoubleFaceIdleState idleState { get; private set; }
    public DoubleFaceMoveState moveState { get; private set; }
    public DoubleFaceBattleState battleState { get; private set; }
    public DoubleFaceBattleIdleState battleIdleState { get; private set; }
    public DoubleFaceAttackState attackState { get; private set; }
    public DoubleFaceStunState stunState { get; private set; }
    public DoubleFaceStunIdleState stunIdleState { get; private set; }
    #endregion



    protected override void Awake()
    {
        base.Awake();

        idleState = new DoubleFaceIdleState(this, stateMachine, "Idle", this);
        moveState = new DoubleFaceMoveState(this, stateMachine, "Move", this);
        battleState = new DoubleFaceBattleState(this, stateMachine, "Move", this);
        battleIdleState = new DoubleFaceBattleIdleState(this, stateMachine, "Idle", this);
        attackState = new DoubleFaceAttackState(this, stateMachine, "Attack", this);
        stunState = new DoubleFaceStunState(this, stateMachine, "Stun", this);
        stunIdleState = new DoubleFaceStunIdleState(this, stateMachine, "Idle", this);
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();
        hit = IsPlayerDetected();
        IsPlayerHit = hit && ((1 << hit.collider.gameObject.layer) & whatIsPlayer) > 0 ? true : false;
    }

    public override bool CanBeStunned(int _counterAttackDir)
    {
        if(base.CanBeStunned(_counterAttackDir))
        {
            stateMachine.ChangeState(stunState);
            return true;
        }
        return false;
    }


    public override void OnDrawGizmosPlayerFrontCheck()
    {
        base.OnDrawGizmosPlayerFrontCheck();

        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + attackDistance * facingDir, playerCheck.position.y));
    }


}
