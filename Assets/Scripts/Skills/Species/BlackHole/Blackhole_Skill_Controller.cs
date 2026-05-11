using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{
    [SerializeField] private float growSpeed;
    [SerializeField] private float pullForce;
    [SerializeField] float cloneAttackCooldown;
    private float cloneAttackTimer;

    private List<Enemy> targets = new List<Enemy>();
    private float originSize;
    private float maxSize;

    private bool canGrow = false;
    private bool canDestroy = false;
    private bool canAttack = false;

    private int attacksAmount;

    private int randDir = 1;
    private int randAttackCount;
    private float randBlance = 0.5f;

    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        
        CloneAttackLogic();

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
            PullEnemy();
        }

        if (canDestroy)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), growSpeed * Time.deltaTime);
            if (transform.localScale.x < originSize)
            {
                Destroy(gameObject);
            }
        }
    }
    #region Blackhole
    public void SetupBlackhole(float _originalSize, float _maxsize, int _attacksAmount)
    {
        canGrow = true;

        canDestroy = false;
        canAttack = false;

        originSize = _originalSize;
        maxSize = _maxsize;
        attacksAmount = _attacksAmount;

        transform.localScale = new Vector2(originSize, originSize);
    }

    private void PullEnemy()
    {
        if (targets == null)
            return;

        targets.RemoveAll(enemy => enemy == null);

        foreach (var enemy in targets)
        {
                enemy?.PullGravity(transform.position, pullForce);
        }
    }

    private void BlackholeDestroy()
    {
        canGrow = false;
        canAttack = false;
        canDestroy = true;

        if (targets == null)
            return;

        foreach (var enemy in targets)
        {
            enemy?.FreezeTimeAll(false);
        }
    }
    #endregion
    #region CloneAttack
    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && canAttack)
        {
            if (targets.Count <= 0)
            {
                BlackholeDestroy();
                return;
            }

            cloneAttackTimer = cloneAttackCooldown;

            CloneRandom();

            


            targets.RemoveAll(enemy => enemy == null);
            if (targets.Count <= 0)
            {
                BlackholeDestroy();
                return;
            }

            Enemy enemy = targets[Random.Range(0, targets.Count)];

            SkillManager.instance.clone.CreateClone(enemy.transform, randDir, randAttackCount, randDir * Vector2.left * 1f);

            attacksAmount--;

            if (attacksAmount <= 0)
            {
                canAttack = false;
                Invoke("BlackholeDestroy", 1f);
            }
        }
    }

    private void CloneRandom()
    {
        int tempDir = randDir;
        if (Random.value < randBlance)  //优化方向逻辑，尽量保持不同方向
            randDir = -1;
        else
            randDir = 1;

        if (tempDir != randDir)
            randBlance = 0.5f;
        else
            randBlance += 0.25f * randDir;

        randAttackCount = Random.Range(1, 4);
    }

    private void AttackTrigger() => canAttack = true;
    #endregion
    #region Trigger
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canGrow)
            return;
        
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            targets.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && targets.Contains(enemy))
        {
            targets.Remove(enemy);
            enemy.StopPullGravity();
        }
    }

    private void AnimationTrigger()
    {
        canGrow = false;
        if (targets == null)
        {
            return;
        }

        targets.RemoveAll(enemy => enemy == null);
        foreach (var enemy in targets)
        {
            enemy?.FreezeTimeAll(true);
        }

        Invoke("AttackTrigger", 1f);
    }
    #endregion
}
