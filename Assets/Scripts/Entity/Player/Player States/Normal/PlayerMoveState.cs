using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        if(!player.isKnocked)
            rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.J))
        {
            stateMachine.ChangeState(player.primaryAttackState);
            return;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            stateMachine.ChangeState(player.tearsAimState);
            return;
        }

        if (xInput == 0)
            stateMachine.ChangeState(player.idleState);

        if (Input.GetKeyDown(KeyCode.Q) && player.counterAttackCooldownTimer < 0)
        {
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }

        if (Input.GetKeyDown(KeyCode.N) && player.skill.blackhole.CanUseSkill())
        {
            stateMachine.ChangeState(player.disappearState);
        }
    }
}
