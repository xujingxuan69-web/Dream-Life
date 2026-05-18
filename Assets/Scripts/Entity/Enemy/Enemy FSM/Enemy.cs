using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private float defaultMoveSpeed;

    [Header("PlayerDetected Info")]
    [SerializeField] protected Transform playerCheck;
    [SerializeField] protected float playerCheckDistance;
    [SerializeField] protected float playerCheckWidth;
    public LayerMask whatIsPlayer;

    [Header("Enemy Tag")]
    [SerializeField] private bool groundEnemy;    //判定是否为飞行/地面敌人


    public EnemyStateMachine stateMachine { get; private set; }
    public string lastAnimBoolName { get; private set; } 

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
        defaultMoveSpeed = moveSpeed;
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

    public virtual void AssignLastAnimName(string _animBoolName)
    {
        lastAnimBoolName = _animBoolName;
    }

    #region FreezeTime
    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    public virtual void FreezeTimeAll(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            anim.speed = 0;
            ConstraintsFreeze(true);
        }
        else
        {
            anim.speed = 1;
            ConstraintsFreeze(false);
        }
    }

    protected virtual IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }
    #endregion
    #region PullGravity
    public void PullGravity(Vector2 _pullPos, float _pullForce)
    {
        anim.speed = 0.3f;

        if (Vector2.Distance(_pullPos, transform.position) < 0.1f)
        {
            ConstraintsFreeze(true);
            return;
        }

        if (groundEnemy && IsGroundDetected())
        {
            if (Mathf.Abs(_pullPos.x - transform.position.x) < 0.1f)
            {
                ConstraintsFreeze(true);
                return;
            }

            float dirX = Mathf.Sign(_pullPos.x - transform.position.x);
            rb.AddForce(new Vector2(dirX * _pullForce, 0));
        }
        else
        {
            if (Mathf.Abs(_pullPos.y - transform.position.y) < 0.3f)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
                rb.gravityScale = 0;
            }
            Vector2 dir = (_pullPos - (Vector2)transform.position).normalized;
            rb.AddForce(dir * _pullForce);
        }
    }

    public void StopPullGravity()
    {
        anim.speed = 1;
        rb.gravityScale = gravity;
        ConstraintsFreeze(false);
    }
    #endregion
    #region CounterAttackWindow
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
    #endregion

    public virtual bool CanBeStunned(int _counterAttackDir)
    {
        if (canBeStunned && _counterAttackDir * -1 == facingDir)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
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
    public virtual RaycastHit2D IsPlayerFrontDetected() => Physics2D.Raycast(playerCheck.position, Vector2.right * facingDir, playerCheckDistance, whatIsPlayer | whatIsGround);
    public virtual RaycastHit2D IsPlayerBehindDetected() => Physics2D.Raycast(playerCheck.position, Vector2.left * facingDir, playerCheckDistance * 0.2f, whatIsPlayer | whatIsGround);

    public virtual void OnDrawGizmosPlayerFrontCheck()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x + playerCheckDistance * facingDir, playerCheck.position.y));
    }
    public void OnDrawGizmosPlayerBehindCheck()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector2(playerCheck.position.x - playerCheckDistance * facingDir * 0.2f, playerCheck.position.y));
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
