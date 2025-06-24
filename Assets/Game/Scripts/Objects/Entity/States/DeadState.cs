using System.Collections;
using System.Collections.Generic;
using AudioSystem;
using StateMachine;
using UnityEngine;

public class DeadState : State<Entity>
{
    public DeadState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }

   

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
        entity.OnDead?.Invoke();
        PoolingManager.Despawn(entity.gameObject);
    }
}
