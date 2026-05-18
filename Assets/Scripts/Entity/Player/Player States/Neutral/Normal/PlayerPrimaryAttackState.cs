using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;

    private float lastTimeAttacked;
    private bool attackInput;
    private float attackInputBuffer = 5f;
    private float attackInputTimer;

    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (comboCounter > 2 || Time.time >= lastTimeAttacked + player.comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("ComboCounter",comboCounter);

        #region Choose attack direction

        float attackDir = player.facingDir;

        if (xInput !=0 && comboCounter == 2)
            attackDir = xInput;

        #endregion

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);

        stateTimer = 1f;
        attackInput = false;
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .1f);
        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }


        if (Input.GetKeyDown(KeyCode.J))
        {
            attackInput = true;
            attackInputTimer = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.L) && player.skill.dash.CanUseSkill())
        {//这里是冲刺取消攻击并创造分身
            player.skill.clone.CreateCloneOnDashStart(comboCounter + 1);
            if (Input.GetAxisRaw("Horizontal") == -player.facingDir)
            {
                player.Flip();
            }
            stateMachine.ChangeState(player.dashState);
        }

        if (triggerCalled)
        {
            if(attackInput && Time.time < attackInputTimer + attackInputBuffer && player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.attackTransState);
            }
            else if (player.IsGroundDetected())
            {
                stateMachine.ChangeState(player.idleState);
            }
            else
            {
                player.manager.jumpExtra = true;
                player.manager.dashExtra = true;
                stateMachine.ChangeState(player.airState);
            }
        }
    }
}
