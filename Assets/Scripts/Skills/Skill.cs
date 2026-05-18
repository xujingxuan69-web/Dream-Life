using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer = -.1f;

    protected Player player;

    private bool cooldownOn = true;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        if (cooldownOn)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        Debug.Log("cooldown " + cooldownTimer);
        return false;
    }

    public virtual void UseSkill()
    {

    }

    public void SetCooldownOn(bool _cooldownOn) => cooldownOn = _cooldownOn;
}
