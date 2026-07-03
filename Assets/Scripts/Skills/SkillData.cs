using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillType
{
    Default,
    DefaultPlus,
    Active,
    Passive
}

[CreateAssetMenu(fileName = "New Skill Data", menuName = "Data/Skill")]
public class SkillData : ScriptableObject
{
    public Sprite skillIcon;
    public string skillName;
    public FormType skillFormType;
    public SkillType skillType;
    public string skillHotKey;
    public int skillFocusCost;
    public int skillHealthCost;
    public float skillCooldown;
    public string skillDescription;
}
