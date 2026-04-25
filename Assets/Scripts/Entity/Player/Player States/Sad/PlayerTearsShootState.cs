using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTearsShootState : PlayerState
{
    public PlayerTearsShootState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
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
        if (triggerCalled)
        {
            float tempXInput = Input.GetAxis("Horizontal");
            float tempYInput = Input.GetAxis("Vertical");
            Vector2 tempInput = new Vector2(tempYInput, tempXInput);
            SkillManager.instance.tears.CreateTears(tempInput);
            stateMachine.ChangeState(player.idleState);
        }
    }
}
