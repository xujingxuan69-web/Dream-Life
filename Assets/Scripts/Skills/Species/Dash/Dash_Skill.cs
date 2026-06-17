using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash_Skill : Skill
{
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (SkillManager.instance.clone.GetCloneOnDashStart())
        {
            player.stats.AddTempModifier(player.stats.untargetable, 1, player.dashDuration);
        }
    }
}
