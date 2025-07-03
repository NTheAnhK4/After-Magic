using System;
using StateMachine;

public class IdleState : State<Entity>
{
    public IdleState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public IdleState(Entity entity, Func<string> animFuncName) : base(entity, animFuncName)
    {
    }


}