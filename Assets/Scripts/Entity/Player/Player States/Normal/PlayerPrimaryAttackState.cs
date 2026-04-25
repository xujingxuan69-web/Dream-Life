using System.Collections;
using System.Collections.Generic;
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


        PlayerManager.instance.comboCounter = comboCounter + 1;
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
                player.jumpExtra = true;
                player.dashExtra = true;
                stateMachine.ChangeState(player.airState);
            }
        }
    }
}
