using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [Header("Blackhole Skill")]
    [SerializeField] private bool blackholeUnlocked;
    [SerializeField] private SkillTreeSlot_UI blackholeUnlockButton;

    [Header("Blackhole Setting")]
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float originalSize;
    [SerializeField] private float maxSize;

    [SerializeField] private Vector2 offset;
    [Space]
    [SerializeField] private int attacksAmount;

    private Blackhole_Skill_Controller currentBlackhole;

    #region Unlock
    private void UnlockBlackhole()
    {
        blackholeUnlocked = true;
    }
    #endregion

    public override bool CanUseSkill()
    {
        if (!blackholeUnlocked)
            return false;

        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        SetCooldownOn(false);

        player.stats.DecreaseFormFocusBy(50);   //!注意删除，仅用于demo演示
        CreateBlackhole();
    }

    protected override void Start()
    {
        base.Start();

        if (blackholeUnlockButton != null)
            blackholeUnlockButton.unlockSkill += UnlockBlackhole;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void CreateBlackhole()
    {
        GameObject newBlackhole = Instantiate(blackholePrefab, (Vector2)player.transform.position + offset, Quaternion.identity);
        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        currentBlackhole.SetupBlackhole(originalSize, maxSize, attacksAmount);
    }

    public bool SkillCompleted()
    {
        if (!currentBlackhole)
            return false;

        if (currentBlackhole.playerStopDisappear)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        if (blackholeUnlockButton != null)
            blackholeUnlockButton.unlockSkill -= UnlockBlackhole;
    }
}
