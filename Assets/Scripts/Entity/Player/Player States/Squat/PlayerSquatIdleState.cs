using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSquatIdleState : PlayerSquatGroundedState
{
    public PlayerSquatIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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

        if (xInput != 0)
            stateMachine.ChangeState(player.squatMoveState);
    }
}
