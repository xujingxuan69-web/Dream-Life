using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearsAimState : PlayerState
{
    public PlayerTearsAimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = 0.5f;

        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
        {
            if(!player.skill.tears.dotsActive)
                player.skill.tears.DotsActive(true);

           player.skill.tears.DotsAim();
        }


        if (rb.velocity.x != 0 && !player.isKnocked)
        {
            rb.velocity = new Vector2(rb.velocity.x * 0.5f, rb.velocity.y);
            if (rb.velocity.x < 0.5f)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            player.skill.tears.DotsActive(false);
            if (stateTimer > 0)
            {
                stateMachine.ChangeState(player.tearsAttackState);
            }
            else
            {
                player.skill.tears.CreateTears(player.facingDir);
                stateMachine.ChangeState(player.tearsShootState);
            }
            return;
        }
    }
}
