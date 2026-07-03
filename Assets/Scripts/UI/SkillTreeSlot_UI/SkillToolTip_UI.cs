using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillToolTip_UI : MonoBehaviour
{
    [SerializeField] private Vector2 transOffset;

    [SerializeField] private Image skillIcon;
    [SerializeField] private Text skillName;
    [SerializeField] private Text skillFormType;
    [SerializeField] private Text skillDescription;

    [SerializeField] private Text skillType;
    [SerializeField] private Text skillHotKey;
    [SerializeField] private Text skillCost1;
    [SerializeField] private Text skillCost2;
    [SerializeField] private Text skillCooldown;

    public void ShowToolTip(SkillData _skillData)
    {
        SetDefaultPanel();

        skillIcon.sprite = _skillData.skillIcon;
        skillName.text = _skillData.skillName;
        skillDescription.text = _skillData.skillDescription;

        skillFormType.text = _skillData.skillFormType.ToString();
        skillFormType.text = GetSkillFormType(_skillData);

        if (_skillData.skillFocusCost == 0)
        {
            if (_skillData.skillHealthCost > 0)
                skillCost1.text = $"生命消耗  {_skillData.skillHealthCost}%";
        }
        else
        {
            skillCost1.text = $"专注消耗  {_skillData.skillFocusCost}";
            if (_skillData.skillHealthCost > 0)
                skillCost2.text = $"生命消耗  {_skillData.skillHealthCost}%";
        }
        skillCooldown.text = _skillData.skillCooldown != 0 ? $"冷却时间  {_skillData.skillCooldown.ToString("F1")}s" : null;


        switch (_skillData.skillType)
        {
            case SkillType.Default:
                skillType.text = "默认技能";
                skillHotKey.text = "按键 " + _skillData.skillHotKey;

                break;
            case SkillType.DefaultPlus:
                skillType.text = "默认技能-升级";
                skillHotKey.text = "按键 " + _skillData.skillHotKey;
                break;
            case SkillType.Active:
                skillType.text = "主动技能";
                break;
            case SkillType.Passive:
                skillType.text = "被动技能";
                break;
        }

        gameObject.SetActive(true);
    }


    public void HideToolTip() => gameObject.SetActive(false);

    private string GetSkillFormType(SkillData _skillData)
    {
        string formTypeText = "";
        switch (_skillData.skillFormType)
        {
            case FormType.Neutral:
                formTypeText = "通用";
                break;
            case FormType.Wrath:
                formTypeText = "怒";
                break;
            case FormType.Grief:
                formTypeText = "悲";
                break;
            case FormType.Calm:
                formTypeText = "静";
                break;
            default:
                formTypeText = "通用";
                break;
        }

        return formTypeText;
    }

    public Vector2 GetFixedPosition(Vector2 _pos)
    {
        float x = _pos.x + transOffset.x * 0.16f * Mathf.Sign(transOffset.x * 0.5f - _pos.x);
        float y = _pos.y - transOffset.y;
        y = Mathf.Clamp(y, transOffset.y * -3f, int.MaxValue);

        return new Vector2(x, y);
    }

    private void SetDefaultPanel()
    {
        skillHotKey.text = null;
        skillCost1.text = null;
        skillCost2.text = null;
    }
}
