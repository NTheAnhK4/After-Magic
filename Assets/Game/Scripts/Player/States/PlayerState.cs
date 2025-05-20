using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class PlayerState : State<Player>
{
    private string animName;
    public PlayerState(Player entity, string animName) : base(entity)
    {
        this.animName = animName;
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        Debug.Log(this.GetType().Name);
        entity.IsAnimationTriggerFinished = false;
        entity.Anim.SetBool(animName, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        entity.Anim.SetBool(animName, false);
    }

    public virtual void AnimationTrigger()
    {
        
    }

    public virtual void AnimationFinishTrigger() => entity.IsAnimationTriggerFinished = true;
}
