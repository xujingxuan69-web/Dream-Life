using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float originalSize;
    [SerializeField] private float maxSize;

    [SerializeField] private Vector2 offset;
    [Space]
    [SerializeField] private int attacksAmount;

    private Blackhole_Skill_Controller currentBlackhole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        SetCooldownOn(false);

        CreateBlackhole();
    }

    protected override void Start()
    {
        base.Start();
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
}
