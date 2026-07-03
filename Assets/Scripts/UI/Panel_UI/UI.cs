using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    protected List<GameObject> panelList = new List<GameObject>();
    protected GameObject defaultPanel;

    protected virtual void Awake()
    {
        SwitchTo(null);
    }

    public virtual void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
        }
    }

    public virtual void SwitchWithKey()
    {
        if (isMenuActive())
        {
            SwitchTo(null);
            return;
        }

        SwitchTo(defaultPanel);
    }

    public virtual bool isMenuActive()
    {
        foreach (var panel in panelList)
        {
            if (panel != null && panel.activeSelf)
                return true;
        }
        return false;
    }
}
