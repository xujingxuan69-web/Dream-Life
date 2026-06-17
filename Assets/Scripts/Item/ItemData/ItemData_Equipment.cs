using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EquipmentType
{
    Gaunlet,    //拳套 +力量 strength
    Amulet,     //项链 +智力 intelligence
    Boots,      //鞋子 +敏捷 agility
    Helmet      //头盔 +体质 vitality
}

public enum QualityType
{ 
    White,
    Blue,
    Purple,
    Gold
}


[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    public QualityType quality;

    [Header("Major Stats")]     //玩家专属
    public int strength;       //力量    1 increase 1    物理攻击力     + 1%    暴击伤害  
    public int intelligence;   //智力    1 increase 1    形态攻击力     + 1%    形态增伤
    public int agility;        //敏捷    1 increase 1%   暴击率     
    public int vitality;       //体质    1 increase 3~5  生命上限

    #region Extra Equipment Stats
    //以下注释供装备参考
    /*[Header("Offensive Stats")]
    public int physicalDamage; //物理攻击力 
    public int formDamage;     //形态攻击力
    public int critChance;     //暴击率    玩家专属
    public int critPower;      //暴击伤害  玩家专属

    [Header("Defensive Stats")]
    public int maxHealth;      //生命上限
    public int armor;          //物理抗性
    public int formResistance; //形态抗性  敌人专属   
    public int reduction;      //减伤率  1 increase 1%   大概率玩家专属     

    [Header("DamageBuff Stats")]
    public int vulnerable;     //易伤    increase 50% damage   敌人专属
    public int weak;           //虚弱    decrease 25% damage   敌人专属
    public int slowdown;       //减速    1 decrease 1% speed    

    [Header("DotDamge Stats")]
    public int dotDamage;      //持续伤害  1 decrease 1 当前血量*/
    #endregion

    

    #region Modifiers
    public override void AddItemEffect()
    {
        base.AddItemEffect();
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);
    }

    public override void RemoveItemEffect()
    {
        base.RemoveItemEffect();
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);
    }
    #endregion
}
