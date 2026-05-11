using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;


    #region Skill Species
    public Dash_Skill dash { get; private set; }
    public Clone_Skill clone { get; private set; }
    public Tears_Skill tears { get; private set; }
    public Blackhole_Skill blackhole { get; private set; }
    #endregion


    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        else
            instance = this;
    }


    private void Start()
    {
        dash = GetComponent<Dash_Skill>();
        clone = GetComponent<Clone_Skill>();
        tears = GetComponent<Tears_Skill>();
    }

    private void Update()
    {
    }
}
