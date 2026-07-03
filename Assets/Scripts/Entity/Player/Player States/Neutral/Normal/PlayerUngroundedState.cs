using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUngroundedState : PlayerState
{
    public PlayerUngroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.L) && player.manager.dashExtra && player.skill.dash.CanUseSkill())
        {
            stateMachine.ChangeState(player.dashState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.squatEnterState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.N) && player.skill.blackhole.CanUseSkill())
        {
            stateMachine.ChangeState(player.disappearState);
        }
    }
}
