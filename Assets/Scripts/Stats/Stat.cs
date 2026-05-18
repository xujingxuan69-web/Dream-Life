using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField] private int baseValue;


    public List<int> modifiers = new List<int>();

    public int GetValue()       //º”À„
    {
        int finalValue = baseValue;

        if (modifiers == null)
            return finalValue;

        foreach (int modifier in modifiers)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public float GetMultiValue()   //≥ÀÀ„
    {

        float finalValue = 1;
        if (modifiers == null)
            return finalValue;

        foreach (int modifier in modifiers)
        {
            finalValue *= (100 - modifier) * .01f;
        }

        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifiers.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifiers.Remove(_modifier);
    }

    public IEnumerable AddModifierTemp(int _modifier, float _seconds)
    {
        modifiers.Add(_modifier);
        yield return new WaitForSeconds(_seconds);
        modifiers.Remove(_modifier);
    }
}
