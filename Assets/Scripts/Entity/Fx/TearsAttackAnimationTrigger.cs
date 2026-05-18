using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class TearsAttackAnimationTrigger : FxAnimationTrigger
{
    private Collider2D cd;
    private List<Enemy> hitEnemies;
    private int attackDir;

    /*private CharacterStats stats;*/
    //先用player的数值往下做

    protected override void Start()
    {
        base.Start();

        cd = GetComponent<Collider2D>();
        /*stats = GetComponent<CharacterStats>();*/
        if (cd != null)
        {
            cd.enabled = false;
        }
    }

    protected override void AnimationTriggerStart()
    {
        base.AnimationTriggerStart();

        attackDir = player.facingDir;
        hitEnemies = new List<Enemy>();

        if (cd != null)
        {
            cd.enabled = true;
        }
    }

    protected override void AnimationTriggerStop()
    {
        base.AnimationTriggerStop();

        if (cd != null)
        {
            cd.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        EnemyStats stats = collision.GetComponent<EnemyStats>();
        if (enemy != null && !hitEnemies.Contains(enemy))
        {
            hitEnemies.Add(enemy);
            //enemy.DamageEffect(attackDir);
            //collision.GetComponent<CharacterStats>().TakeDamage();
        }
    }
}
