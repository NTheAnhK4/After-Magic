using StateMachine;

public class IdleState : State<Entity>
{
    public IdleState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        //entity.HasFinishedUsingCard = false;
    }
}