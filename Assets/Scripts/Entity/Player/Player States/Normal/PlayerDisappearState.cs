using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisappearState : PlayerState
{
    public PlayerDisappearState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();

        PlayerManager.instance.canDash = false;
        player.ConstraintsFreeze(true);
        player.CollidersFreeze(true);  
        //锁定角色，无法被攻击与移动
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.skill.blackhole.SkillCompleted())
        {
            stateMachine.ChangeState(player.showState);
        }
    }
}
