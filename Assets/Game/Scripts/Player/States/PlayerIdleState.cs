using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class PlayerIdleState : PlayerState
{
    public PlayerIdleState(Player entity, string animName) : base(entity, animName)
    {
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        entity.RB.velocity = new Vector2(0, 0);
    }
}
