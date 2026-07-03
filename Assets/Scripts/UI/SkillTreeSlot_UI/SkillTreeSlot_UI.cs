using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillTreeSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private int skillPrice;

    public bool unlocked;

    [SerializeField] private SkillTreeSlot_UI[] shouldBeUnlocked;
    [SerializeField] private SkillTreeSlot_UI[] shouldBeLocked;

    [SerializeField] private Image skillIcon;
    [SerializeField] private Image lockImage;
    [SerializeField] private GameObject pricePanel;
    [SerializeField] private Text priceText;

    private SkillTree_UI ui;

    public System.Action unlockSkill;

    private void OnValidate()
    {
        gameObject.name = "SkillTreeSlot_UI - " + skillData?.skillName;
    }

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(() => UnlockSkillSlot());
    }

    private void Start()
    {
        LockImage();
        ui = GetComponentInParent<SkillTree_UI>();
    }

    private void LockImage()
    {
        skillIcon.sprite = skillData.skillIcon;
        pricePanel.SetActive(false);
        if (!unlocked)
        {
            skillIcon.color = Color.gray;
            lockImage.enabled = true;
        }
        else
        {
            skillIcon.color = Color.white;
            lockImage.enabled = false;
        }
    }

    public void UnlockSkillSlot()
    {
        for (int i = 0; i < shouldBeUnlocked.Length; i++)
        {
            if (shouldBeUnlocked[i].unlocked == false)
            {
                return;
            }
        }

        for (int i = 0; i < shouldBeLocked.Length; i++)
        {
            if (shouldBeLocked[i].unlocked == true)
            {
                return;
            }
        }

        if (!PlayerManager.instance.HaveEnoughCurrency(skillPrice))
            return;

        pricePanel.SetActive(false);
        unlocked = true;
        skillIcon.color = Color.white;
        lockImage.enabled = false;
        unlockSkill?.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!unlocked)
        {
            priceText.text = skillPrice.ToString();
            pricePanel.SetActive(true);
        }

        ui.skillToolTip.transform.position = ui.skillToolTip.GetFixedPosition(transform.position);
        ui.skillToolTip.ShowToolTip(skillData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pricePanel.SetActive(false);
        ui.skillToolTip.HideToolTip();
    }

}
