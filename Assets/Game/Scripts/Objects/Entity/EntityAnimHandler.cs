using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class EntityAnimHandler : ComponentBehaviour
{
    [SerializeField] private Entity entity;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (entity == null) entity = transform.GetComponentInParent<Entity>();
    }
    protected void AnimationTrigger() => entity.OrNull()?.StateMachine.State.AnimationTrigger();
    protected void AnimationFinishTrigger() => entity.OrNull()?.StateMachine.State.AnimationFinishTrigger();

}
