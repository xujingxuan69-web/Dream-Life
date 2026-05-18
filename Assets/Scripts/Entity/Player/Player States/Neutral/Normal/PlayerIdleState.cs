using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (rb.velocity.x != 0 && !player.isKnocked)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
            if (rb.velocity.x < 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (xInput != 0 && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            stateMachine.ChangeState(player.squatEnterState);
        }

        #region Idle&Move Share
        if (Input.GetKeyDown(KeyCode.Q) && player.counterAttackCooldownTimer < 0)
        {
            stateMachine.ChangeState(player.counterAttackState);
            return;
        }

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

        if (Input.GetKeyDown(KeyCode.N) && player.skill.blackhole.CanUseSkill())
        {
            stateMachine.ChangeState(player.disappearState);
            return;
        }
        #endregion
    }
}
