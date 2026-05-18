using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int facingDir { get; private set; } = 1;
    protected bool facingRight = true;

    public float gravity = 2;

    private RigidbodyConstraints2D originalConstraints;

    [Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackDirection;
    [SerializeField] protected float knockbackDuration;
    public bool isKnocked { get; private set; }
    public bool isInvincible { get; private set; }

    [Header("Collision Info")]
    public Transform attackCheck;
    public float attackCheckRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float groundCheckWidth;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected float wallCheckWidth;
    [SerializeField] protected LayerMask whatIsGround;

    #region Components
    public EntityFx fx { get; private set; }
    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer[] sr { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public Collider2D[] colliders { get; private set; }
    
    public CharacterStats stats { get; private set; }
    #endregion

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        fx = GetComponent<EntityFx>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CapsuleCollider2D>();
        colliders = GetComponents<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentsInChildren<SpriteRenderer>();
        stats = GetComponent<CharacterStats>();

        rb.gravityScale = gravity;
        originalConstraints = rb.constraints;
        isInvincible = false;
    }

    protected virtual void Update()
    {

    }

    public virtual void DamageEffect(int attackerDirection)
    {
        fx.StartCoroutine("FlashFx");
        StartCoroutine(HitKnockback(attackerDirection));
    }

    protected virtual IEnumerator HitKnockback(int attackerDirection)
    {
        isKnocked = true;

        if (attackerDirection != 0)
            rb.velocity = new Vector2(knockbackDirection.x * attackerDirection, knockbackDirection.y + rb.velocity.y * 0.5f);

        yield return new WaitForSeconds(knockbackDuration);

        isKnocked = false;
    }

    public void ConstraintsFreeze(bool _freeze) => rb.constraints = _freeze ? 
        rb.constraints = RigidbodyConstraints2D.FreezeAll : rb.constraints = originalConstraints; 

    public void CollidersFreeze(bool _freeze)
    {
        for (int i = 0; i < colliders.Length; i++)
            colliders[i].enabled = !_freeze;
    }

    #region Velocity
    public void SetZeroVelocity()
    {
        if (isKnocked)
        {
            return;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void SetVelocity(float _xVelocity, float _yVelocity)
    {
        if (isKnocked)
        {
            return;
        }

        rb.velocity = new Vector2(_xVelocity, _yVelocity);
        FlipController(_xVelocity);
    }
    #endregion
    #region Flip 
    public virtual void Flip()
    {
        if (rb.constraints == RigidbodyConstraints2D.FreezeAll)
            return;

        facingDir *= -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void FlipController(float _x)
    {
        if ((_x > 0 && !facingRight) || (_x < 0 && facingRight))
        {
            Flip();
        }
    }
    #endregion
    #region Check
    public virtual bool IsGroundDetected() => Physics2D.OverlapBox(new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector3(groundCheckWidth, groundCheckDistance), 0, whatIsGround);

    public virtual bool IsWallDetected() => Physics2D.OverlapBox(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth), 0, whatIsGround);

    public virtual void OnDrawGizmos()
    {
        OnDrawGizmosGroundCheck();
        OnDrawGizmosWallCheck();
        OnDrawGizmosAttackCheck();
    }

    public virtual void OnDrawGizmosWallCheck()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(wallCheck.position.x + facingDir * wallCheckDistance * 0.5f, wallCheck.position.y),
            new Vector3(wallCheckDistance, wallCheckWidth));
    }

    public virtual void OnDrawGizmosGroundCheck()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance * 0.5f),
            new Vector3(groundCheckWidth, groundCheckDistance));
    }

    public virtual void OnDrawGizmosAttackCheck()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }
    #endregion

    public void MakeTransparent(bool _Transparent)  //瞬间隐形
    {
        if (_Transparent)
        {
            for (int i =0; i < sr.Length; i++)  //for的性能略优于foreach，所以在此使用
                sr[i].color = Color.clear;
        }
        else
        {
            for (int i = 0; i < sr.Length; i++)
                sr[i].color = Color.white;
        }
    }

    public void SetInvincible(bool _isInvincible) => isInvincible = _isInvincible;

    public virtual void Die()
    {

    }
}
