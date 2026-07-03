using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    #region Skill Unlock
    [Header("Clone on Dash Start")]
    [SerializeField] private bool cloneOnDashStartUnlocked;
    [SerializeField] private SkillTreeSlot_UI cloneOnDashStartUnlockButton;

    [Header("Clone on Attack")]
    [SerializeField] private bool cloneOnAttackUnlocked;
    [SerializeField] private SkillTreeSlot_UI cloneOnAttackUnlockButton;

    protected override void Start()
    {
        base.Start();
        cloneOnDashStartUnlockButton.unlockSkill += UnlockCloneOnDashStart;
        cloneOnAttackUnlockButton.unlockSkill += UnlockCloneOnAttack;
    }

    #region Unlock
    private void UnlockCloneOnDashStart()
    {
        if (cloneOnDashStartUnlockButton.unlocked)
            cloneOnDashStartUnlocked = true;
        else
            Debug.Log("not unlock skill");
    }

    private void UnlockCloneOnAttack()
    {
        if (cloneOnAttackUnlockButton.unlocked)
            cloneOnAttackUnlocked = true;
    }
    #endregion

    public void CreateCloneOnDashStart(int _comboCounter = 0)
    {
        if (cloneOnDashStartUnlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, player.facingDir, _comboCounter);
        }
    }

    public void CreateCloneOnCounterAttack(Transform _enemyTransform, int _facingDir)
    {
        if (cloneOnAttackUnlocked)
        {
            SkillManager.instance.clone.StartCoroutine(SkillManager.instance.clone.CreateCloneWithDelay(_enemyTransform, _facingDir, _facingDir * Vector2.right));
        }
    }
    #endregion

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();
        if (cloneOnDashStartUnlocked)
        {
            player.stats.AddTempModifier(player.stats.untargetable, 1, player.dashDuration);
        }
    }

    private void OnDestroy()
    {
        cloneOnDashStartUnlockButton.unlockSkill -= UnlockCloneOnDashStart;
        cloneOnAttackUnlockButton.unlockSkill -= UnlockCloneOnAttack;
    }
}
