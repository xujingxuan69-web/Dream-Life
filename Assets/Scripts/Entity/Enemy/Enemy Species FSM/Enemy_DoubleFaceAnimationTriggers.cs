using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DoubleFaceAnimationTriggers : MonoBehaviour
{
    private Enemy_DoubleFace enemy => GetComponentInParent<Enemy_DoubleFace>();

    private void AnimationTrigger()
    {
        enemy.AnimationFinishTrigger();
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            Player curPlayer = hit.GetComponent<Player>();
            if (curPlayer != null && curPlayer.isInvincible == false)
            {
                PlayerStats _target = hit.GetComponent<PlayerStats>();
                enemy.stats.DoDamage(_target, enemy.facingDir);
                return;
            }
        }
    }


    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
