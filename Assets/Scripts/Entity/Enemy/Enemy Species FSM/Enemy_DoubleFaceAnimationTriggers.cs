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
        bool hasAttackPlayer = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                hit.GetComponent<Player>().Damage(enemy.facingDir);
                hasAttackPlayer = true;

                
            }
            if (hasAttackPlayer)
            {
                return;
            }
        }
    }


    private void OpenCounterWindow() => enemy.OpenCounterAttackWindow();
    private void CloseCounterWindow() => enemy.CloseCounterAttackWindow();
}
