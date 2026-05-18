using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSquatEnterState : PlayerState
{
    public PlayerSquatEnterState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 0.3f;
        player.tempMoveSpeed = player.moveSpeed; 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.velocity.y);

        if (Input.GetKeyUp(KeyCode.LeftControl) && stateTimer > 0.1f)
        {
            if (player.IsGroundDetected())
                stateMachine.ChangeState(player.idleState);
            else
                stateMachine.ChangeState(player.airState);
        }

        if (stateTimer < 0)
        {
            
            player.normalCollider.enabled = false;
            player.squatEnter = true;
            if (!player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.squatAirState);
            }
            else
            {
                player.tempMoveSpeed = player.moveSpeed * player.moveSpeedRate;
                stateMachine.ChangeState(player.squatIdleState);
            }
        }
    }
}
