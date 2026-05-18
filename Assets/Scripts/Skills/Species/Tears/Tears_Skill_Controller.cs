using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Tears_Skill_Controller : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private float tearsScale;
    private float tearsScaleSpeed;
    private float gravityScale;

    [Header("Bounce Info")]
    [SerializeField] private float bounceSpeed;
    [SerializeField] private float bounceDistance;
    private bool isBouncing;
    private int bounceAmount;


    
    private float freezeTimeDuration;

    
    
    private bool isPiercing;
    private int pierceAmount;

    private List<Enemy> enemyTarget;
    private int targetIndex;

    private void Awake()
    {
        this.enabled = false;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();
    }

    public void SetupTears(Vector2 _dir, float _gravityScale,float _gravityTime, float _tearsScale, 
        float _tearsScaleSpeed, float _freezeTimeDuration, float _destroyDuration)
    {
        gravityScale = _gravityScale;
        rb.gravityScale = 0;
        rb.velocity = _dir;

        tearsScale = _tearsScale;
        tearsScaleSpeed = _tearsScaleSpeed;

        freezeTimeDuration = _freezeTimeDuration;

        Invoke("TearsGravity", _gravityTime);
        Invoke("TearsDestroy", _destroyDuration);
        this.enabled = true;
    }

    public void SetupBounce(bool _isBouncing, int _BounceAmount)
    {
        isBouncing = _isBouncing;
        bounceAmount = _BounceAmount;

        enemyTarget = new List<Enemy>();
    }

    public void SetupPierce(bool _isPiercing, int _pierceAmount)
    {
        isPiercing = _isPiercing;
        pierceAmount = _pierceAmount;
    }

    private void Update()
    {

        if (transform.localScale.x < tearsScale)
        {
            transform.localScale = new Vector2(transform.localScale.x + Time.deltaTime * tearsScaleSpeed,
                transform.localScale.y + Time.deltaTime * tearsScaleSpeed);
            if (transform.localScale.x > tearsScale)
                transform.localScale = new Vector2(tearsScale, tearsScale);
        }

        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle); //移动时的旋转

        BounceLogic();
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            Enemy targetEnemy = enemyTarget[targetIndex];

            if (targetEnemy == null)
            {
                targetIndex++;
                return;
            }

            Vector2 dir = (targetEnemy.transform.position - transform.position).normalized;
            rb.velocity = dir * bounceSpeed;
            
            if (Vector2.Distance(transform.position, targetEnemy.transform.position) > bounceDistance)
                TearsDestroy();

            if (Vector2.Distance(transform.position, targetEnemy.transform.position) < .5f)
            {
                targetIndex++;
                bounceAmount--;

                if (bounceAmount <= 0)
                    TearsDestroy();

                if (targetIndex >= enemyTarget.Count)
                    targetIndex = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy curEnemy = collision.GetComponent<Enemy>();

        //curEnemy?.Damage(Vector2.Dot(rb.velocity.normalized, Vector2.right) > 0? 1 : -1);

        if (curEnemy != null)
        {
            TearsSkillDamage(curEnemy);

            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceDistance);

                foreach (var hit in colliders)
                {
                    Enemy enemy = hit.GetComponent<Enemy>();
                    if (enemy != null && enemy != curEnemy)
                        enemyTarget.Add(enemy);
                }
                if (enemyTarget.Count != 0)
                    enemyTarget.Add(curEnemy);
            }
        }
        else
        {
            TearsDestroy();
        }

        if (isBouncing && enemyTarget.Count > 0)
            return;

        if (isPiercing && pierceAmount > 0)
        {
            pierceAmount--;
            return;
        }

        TearsDestroy();

    }

    private void TearsSkillDamage(Enemy curEnemy)
    {
        curEnemy.DamageEffect((int)Mathf.Sign(rb.velocity.x));
        curEnemy.StartCoroutine("FreezeTimeFor", freezeTimeDuration);
    }

    private void TearsGravity()
    {
        rb.gravityScale = gravityScale;
    }

    private void TearsDestroy()
    {
        Destroy(gameObject);
    }
}
