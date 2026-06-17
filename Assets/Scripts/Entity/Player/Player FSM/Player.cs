using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    #region Manager
    public PlayerManager manager { get; private set; }
    public SkillManager skill { get; private set; }
    #endregion
    #region Inspector
    public bool isBusy { get; private set; }

    [Header("Move Info")]
    public float moveSpeed = 5;
    public float squatMoveSpeedRate = 1.5f;
    private float defaultMoveSpeed;
    

    [Header("Jump Info")]
    public float jumpForce = 12;

    public float jumpAirDuration = 0.1f;
    private float lastJumpAirTime;    //土狼时间计时
    
    
    public float wallJumpDuration = 0.5f;
    private float defaultJumpForce;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    private float defaultDashSpeed;

    [Header("Attack Info")]
    public float comboWindow = 0.3f;
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    [SerializeField] private float counterAttackCooldown = .7f;
    private float lastCounterAttackTime;

    [Header("Collider Info")]
    public CapsuleCollider2D normalCollider;
    public CircleCollider2D squatCollider;

    [Header("Squat Info")]
    [SerializeField] protected Transform headCheck;
    [SerializeField] protected float headCheckDistance;
    [SerializeField] protected float headCheckWidth;
    [HideInInspector] public float squatMoveSpeed;
    #endregion
    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    #region normalState
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }

    public PlayerWallSlideState wallSlideState { get; private set; }

    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }

    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerAttackTransState attackTransState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }

    public PlayerDisappearState disappearState { get; private set; }
    public PlayerShowState showState { get; private set; }

    public PlayerDeadState deadState { get; private set; }
    public PlayerDeadAirState deadAirState { get; private set; }
    #endregion
    #region squatState
    public PlayerSquatEnterState squatEnterState { get; private set; }
    public PlayerSquatIdleState squatIdleState { get; private set; }
    public PlayerSquatMoveState squatMoveState { get; private set; }
    public PlayerSquatJumpState squatJumpState { get; private set; }
    public PlayerSquatAirState squatAirState { get; private set; }
    public PlayerSquatExitState squatExitState { get; private set; }
    #endregion
    #region tearsState
    public PlayerTearsAimState tearsAimState { get; private set; }
    public PlayerTearsShootState tearsShootState { get; private set; }
    public PlayerTearsAttackState tearsAttackState { get; private set; }
    #endregion
    #endregion
    #region Component
    public ItemDrop itemDrop { get; private set; }
    public PlayerFx playerFx { get; private set; }
    public PlayerStats stats { get; private set; }
    #endregion
    #region Action
    public System.Action onHealthFlaskUsed;
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        attackTransState = new PlayerAttackTransState(this, stateMachine, "AttackTrans");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        disappearState = new PlayerDisappearState(this, stateMachine, "Disappear");
        showState = new PlayerShowState(this, stateMachine, "Show");

        deadState = new PlayerDeadState(this, stateMachine, "Die");
        deadAirState = new PlayerDeadAirState(this, stateMachine, "Jump");

        squatEnterState = new PlayerSquatEnterState(this, stateMachine, "SquatEnter");
        squatIdleState = new PlayerSquatIdleState(this, stateMachine, "Squat");
        squatMoveState = new PlayerSquatMoveState(this, stateMachine, "Squat");
        squatJumpState = new PlayerSquatJumpState(this, stateMachine, "Squat");
        squatAirState = new PlayerSquatAirState(this, stateMachine, "Squat");
        squatExitState = new PlayerSquatExitState(this, stateMachine, "SquatExit");

        tearsAimState = new PlayerTearsAimState(this, stateMachine, "TearsAim");
        tearsShootState = new PlayerTearsShootState(this, stateMachine, "TearsShoot");
        tearsAttackState = new PlayerTearsAttackState(this, stateMachine, "TearsAttack");
    }

    protected override void Start()
    {
        base.Start();
        itemDrop = GetComponent<ItemDrop>();
        playerFx = GetComponent<PlayerFx>();
        stats = GetComponent<PlayerStats>();

        manager = PlayerManager.instance;
        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);

        defaultMoveSpeed = moveSpeed;
        defaultJumpForce = jumpForce;
        defaultDashSpeed = dashSpeed;

        lastJumpAirTime = -jumpAirDuration;
        lastCounterAttackTime = -counterAttackCooldown;

        SetSquatCollider(false);
    }

    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            onHealthFlaskUsed?.Invoke();
        }
    }

    #region JumpAirTime
    public void SetJumpAirTime() => lastJumpAirTime = Time.time;

    public bool CheckJumpAirTime() => Time.time <= lastJumpAirTime + jumpAirDuration;
    #endregion
    #region CounterAttackTime
    public void SetCounterAttackTime() => lastCounterAttackTime = Time.time;

    public bool CheckCounterAttackTime() => Time.time > lastCounterAttackTime + counterAttackCooldown;
    #endregion

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void SetSquatCollider(bool _set)
    {
        if (_set)
        {
            normalCollider.enabled = false;
            squatCollider.enabled = true;
        }
        else
        {
            normalCollider.enabled = true;
            squatCollider.enabled = false;
        }
    }   //切换下蹲状态的Collider

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    #region Collision Checks
    public override bool IsGroundDetected()
    {
        return base.IsGroundDetected();
    }

    public bool IsWallSlideDetected() => Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.7f), 0, whatIsGround)
        && Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y + wallCheckWidth * 0.425f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.15f), 0, whatIsGround)
        && Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y - wallCheckWidth * 0.425f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.15f), 0, whatIsGround);

    public bool IsHeadDetected() => Physics2D.OverlapBox(new Vector3(headCheck.position.x, headCheck.position.y + headCheckDistance * 0.5f),
            new Vector3(headCheckWidth, headCheckDistance), 0, whatIsGround);

    public override void OnDrawGizmosWallCheck()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.7f));
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y + wallCheckWidth * 0.425f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.15f));
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y - wallCheckWidth * 0.425f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.15f));

        Gizmos.DrawWireCube(new Vector3(headCheck.position.x, headCheck.position.y + wallCheckDistance * 0.5f),
            new Vector3(headCheckWidth, headCheckDistance));
    }
    #endregion

    public override void Die()
    {
        base.Die();

        stateMachine.ChangeState(deadState);
    }

    #region Speed
    public override void SpeedSlowBy(float _slowPercentage)
    {
        base.SpeedSlowBy(_slowPercentage);

        moveSpeed = defaultMoveSpeed * (1 - _slowPercentage);
        jumpForce = defaultJumpForce * (1 - _slowPercentage);
        dashSpeed = defaultDashSpeed * (1 - _slowPercentage);
    }

    public override void SpeedReturnDefault()
    {
        base.SpeedReturnDefault();

        moveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
        dashSpeed = defaultDashSpeed;
    }
    #endregion

    
}
