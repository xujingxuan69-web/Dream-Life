using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    [SerializeField] private bool invincibleDash;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (invincibleDash)
        {
            StartCoroutine(InvincibleDash());
        }
    }

    private IEnumerator InvincibleDash()
    {
        Debug.Log("true");
        player.SetInvincible(true);
        yield return new WaitForSeconds(player.dashDuration);
        player.SetInvincible(false);

    }
}
