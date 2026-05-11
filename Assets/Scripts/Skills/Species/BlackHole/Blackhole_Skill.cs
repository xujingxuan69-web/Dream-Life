using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float originalSize;
    [SerializeField] private float maxSize;
    [Space]
    [SerializeField] private int attacksAmount;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        CreateBlackhole();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        
    }

    private void CreateBlackhole()
    {
        GameObject newBlackhole = Instantiate(blackholePrefab, (Vector2)player.transform.position, Quaternion.identity);
        Blackhole_Skill_Controller newBlackholeScript = newBlackhole.GetComponent<Blackhole_Skill_Controller>();
        newBlackholeScript.SetupBlackhole(originalSize, maxSize, attacksAmount);
    }
}
