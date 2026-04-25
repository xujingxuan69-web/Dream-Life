using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    private string animBoolName;
    private AnimatorControllerParameterType animType;


    protected Rigidbody2D rb;

    protected float xInput;
    protected float yInput;


    protected float stateTimer;
    protected bool triggerCalled;
    
    public PlayerState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
        this.animBoolName = _animBoolName;

        
    }

    public virtual bool IsAttackState() => false;

    public virtual void Enter()
    {
        foreach (var _animParam in player.anim.parameters)
        {
            if (_animParam.name == animBoolName)
            {
                animType = _animParam.type;
                break;
            }
        }

        switch (animType)
        {
            case AnimatorControllerParameterType.Bool:
                player.anim.SetBool(animBoolName, true);
                break;
            case AnimatorControllerParameterType.Trigger:
                player.anim.SetTrigger(animBoolName);
                break;
        }
            
        rb = player.rb;

        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        
        player.anim.SetFloat("yVelocity", rb.velocity.y);


    }

    public virtual void Exit()
    {
        switch (animType)
        {
            case AnimatorControllerParameterType.Bool:
                player.anim.SetBool(animBoolName, false);
                break;
            case AnimatorControllerParameterType.Trigger:
                player.anim.ResetTrigger(animBoolName);
                break;
        }
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
