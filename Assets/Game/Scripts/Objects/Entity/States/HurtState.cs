using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class HurtState : State<Entity>
{
    public HurtState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }
    
    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        entity.StateMachine.ChangeState(entity.IdleState);
    }

    public override void OnExit()
    {
        base.OnExit();
        entity.IsHurting = false;
    }
}
