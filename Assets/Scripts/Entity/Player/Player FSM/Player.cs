using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity
{
    #region SkillManager
    public SkillManager skill { get; private set; }


    #endregion
    #region Inspector
    public bool isBusy { get; private set; }
    [Header("Move Info")]
    public float moveSpeed = 5;
    public float moveSpeedRate = 1.5f;
    [HideInInspector]public float tempMoveSpeed = 5;
    

    [Header("Jump Info")]
    public float jumpForce = 12;
    [HideInInspector] public bool jumpExtra;
    public float jumpDuration = 0.1f;
    public float jumpTimer { get; private set; }
    public float wallJumpDuration = 0.5f;

    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDir {get; private set; }
    public float dashDuration;
    [SerializeField] private float dashCoolDown;
    private float dashCoolDownTimer;

    [HideInInspector] public bool dashExtra;

    [Header("Attack Info")]
    public float comboWindow = 0.3f;
    public Vector2[] attackMovement;
    public float counterAttackDuration = .2f;
    [SerializeField] private float counterAttackCoolDown = .5f;
    public float counterAttackCooldownTimer { get; private set; }

    [Header("Collider Info")]
    public Collider2D normalCollider;

    [Header("Squat Info")]
    [SerializeField] protected Transform headCheck;
    [SerializeField] protected float headCheckDistance;
    [SerializeField] protected float headCheckWidth;
    [HideInInspector] public bool squatEnter;
    #endregion
    #region States
    public PlayerStateMachine stateMachine { get; private set; }

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

    public PlayerSquatEnterState squatEnterState { get; private set; }
    public PlayerSquatIdleState squatIdleState { get; private set; }
    public PlayerSquatMoveState squatMoveState { get; private set; }
    public PlayerSquatJumpState squatJumpState { get; private set; }
    public PlayerSquatAirState squatAirState { get; private set; }
    public PlayerSquatExitState squatExitState { get; private set; }

    public PlayerTearsAimState tearsAimState { get; private set; }
    public PlayerTearsShootState tearsShootState { get; private set; }
    public PlayerTearsAttackState tearsAttackState { get; private set; }
    #endregion

    #region playerFx
    public PlayerFx playerFx { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        stateMachine = new PlayerStateMachine();
        playerFx = GetComponent<PlayerFx>();

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

        skill = SkillManager.instance;

        stateMachine.Initialize(idleState);
    }

    protected override void Update()
    {
        base.Update();

        jumpTimer -= Time.deltaTime;
        dashCoolDownTimer -= Time.deltaTime;
        counterAttackCooldownTimer -= Time.deltaTime;

        stateMachine.currentState.Update();

        CheckForDashInput();
        
    }

    public override void Damage(int attackerDirection)
    {
        base.Damage(attackerDirection);
        Debug.Log("Player Damage");
    }

    public void SetJumpTimer()
    {
        jumpTimer = jumpDuration;
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    public void SetCounterAttackTimer() => counterAttackCooldownTimer = counterAttackCoolDown;

    private void CheckForDashInput()
    {
        if (stateMachine.currentState.IsAttackState() || (squatEnter && IsHeadDeatected()))
            return;


        if (Input.GetKeyDown(KeyCode.L) && stateMachine.currentState == wallSlideState)
        {
            dashExtra = false;
            jumpExtra = true;
            
            dashDir = facingDir*-1;

            stateMachine.ChangeState(dashState);
        }
        else if (Input.GetKeyDown(KeyCode.L) && stateMachine.currentState == primaryAttackState)
        {
            if (!skill.clone.canAttack)
            {
                return;
            }
            else if (skill.dash.CanUseSkill())
            {
                dashExtra = false;

                dashDir = Input.GetAxisRaw("Horizontal") == 0 ? facingDir : Input.GetAxisRaw("Horizontal");

                PlayerManager.instance.attackDash = true;
                stateMachine.ChangeState(dashState);
            }
        }
        else if (Input.GetKeyDown(KeyCode.L) && skill.dash.CanUseSkill() && (IsGroundDetected() || dashExtra))
        {
            dashExtra = false;

            dashDir = Input.GetAxisRaw("Horizontal") == 0 ? facingDir : Input.GetAxisRaw("Horizontal");

            stateMachine.ChangeState(dashState);
        }
    }

    

    #region Collision Checks
    public override bool IsWallDetected() => Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.6f), 0, whatIsGround)
        && Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y + wallCheckWidth * 0.35f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.1f), 0, whatIsGround)
        && Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y - wallCheckWidth * 0.35f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.1f), 0, whatIsGround);

    public virtual bool IsHeadDeatected() => Physics2D.OverlapBox(new Vector3(headCheck.position.x, headCheck.position.y + headCheckDistance * 0.5f),
            new Vector3(headCheckWidth, headCheckDistance), 0, whatIsGround);

    public override void OnDrawGizmosWallCheck()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.6f));
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y + wallCheckWidth * 0.35f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.1f));
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y - wallCheckWidth * 0.35f),
            new Vector3(wallCheckDistance, wallCheckWidth * 0.1f));

        Gizmos.DrawWireCube(new Vector3(headCheck.position.x, headCheck.position.y + wallCheckDistance * 0.5f),
            new Vector3(headCheckWidth, headCheckDistance));
    }
    #endregion
}
