using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTree_UI : UI
{
    [SerializeField] private GameObject neutralPanel;
    [SerializeField] private GameObject wrathPanel;
    [SerializeField] private GameObject griefPanel;
    [SerializeField] private GameObject calmPanel;

    public SkillToolTip_UI skillToolTip;

    protected override void Awake()
    {

        base.Awake();
        defaultPanel = neutralPanel;
        panelList.Add(neutralPanel);
        panelList.Add(wrathPanel);
        panelList.Add(calmPanel);
        panelList.Add(calmPanel);
    }
}
