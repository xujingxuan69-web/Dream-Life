using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Entity
{
    [Header("Stun Info")]
    public float stunDuration;
    public float stunIdleDuration;
    public Vector2 stunDirection;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;


    [Header("Move Info")]
    public float moveSpeed = 2f;
    public float idleTime = 1f;

    [Header("PlayerDetected Info")]
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected float playerCheckWidth;
    public LayerMask whatIsPlayer;

    


    public EnemyStateMachine stateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned= false ;
        counterImage.SetActive(false);
    }


    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    public override void Damage(int attackerDirection)
    {
        base.Damage(attackerDirection);
        Debug.Log("I Damage");
    }
    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    #region Check
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        OnDrawGizmosPlayerFrontCheck();
        OnDrawGizmosPlayerBehindCheck();
    }
    #region Player Check
    public virtual bool IsPlayerInSight() => Physics2D.OverlapBox(
        new Vector2(playerCheck.position.x + facingDir * playerCheckDistance * 0.5f, playerCheck.position.y),
        new Vector2(playerCheckDistance, playerCheckWidth), 0, whatIsPlayer);
    public virtual bool IsPlayerBehindSight() => Physics2D.OverlapBox(
        new Vector2(playerCheck.position.x - facingDir * playerCheckDistance * 0.5f, playerCheck.position.y),
        new Vector2(playerCheckDistance, playerCheckWidth), 0, whatIsPlayer);
    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
    

    public virtual void OnDrawGizmosPlayerFrontCheck()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(playerCheck.position.x + facingDir * playerCheckDistance * 0.5f, playerCheck.position.y),
            new Vector2(playerCheckDistance, playerCheckWidth));
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
    }
    public void OnDrawGizmosPlayerBehindCheck()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(playerCheck.position.x - facingDir * playerCheckDistance * 0.5f, playerCheck.position.y),
            new Vector2(playerCheckDistance, playerCheckWidth));
    }
    #endregion
    #region Collisions Check
    public override bool IsGroundDetected() => Physics2D.OverlapBox(new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.6f, groundCheckDistance), 0, whatIsGround);

    public bool IsGroundFrontDetected() => Physics2D.OverlapBox(new Vector2(groundCheck.position.x + groundCheckWidth * 0.35f * facingDir, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.1f, groundCheckDistance), 0, whatIsGround);
    public bool IsGroundBehindDetected() => Physics2D.OverlapBox(new Vector2(groundCheck.position.x - groundCheckWidth * 0.35f * facingDir, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.1f, groundCheckDistance), 0, whatIsGround);

    public override void OnDrawGizmosGroundCheck()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.6f, groundCheckDistance));
        Gizmos.DrawWireCube(new Vector2(groundCheck.position.x + groundCheckWidth * 0.35f * facingDir, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.1f, groundCheckDistance));
        Gizmos.DrawWireCube(new Vector2(groundCheck.position.x - groundCheckWidth * 0.35f * facingDir, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector2(groundCheckWidth * 0.1f, groundCheckDistance));
    }
    #endregion
    #endregion
}
