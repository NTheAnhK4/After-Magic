
using StateMachine;
using UnityEngine;


public class UseCardState : State<Entity>
{
    private CardStrategy Card;
    public UseCardState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
        
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        if (entity.CardStrategy != null)
        {
            Card = entity.CardStrategy;
            entity.Anim.SetBool(Card.AnimName, true);
            Card.Apply(entity, entity.EnemyTarget);
           
        }
        
       
    }

    public override void Update()
    {
        base.Update();
        Card = entity.CardStrategy;
        if (Card == null) return;
        if (!entity.CardStrategy.HasFinisedUsingCard()) return;
        
        if(entity.MustReachTarget) entity.StateMachine.ChangeState(entity.RunState, () => new RunStateData(){IsRunningToTarget = false, TargetPosition = entity.StandPoint});
        else
        {
            entity.OnFinishedUsingCard?.Invoke();
            entity.StateMachine.ChangeState(entity.IdleState);
        }
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if(Card != null) Card.AnimationTrigger();
    }

    public override void OnExit()
    {
        base.OnExit();
        if (Card != null)
        {
            entity.Anim.SetBool(Card.AnimName, false);
            Card.Remove();
            entity.CardStrategy = null;
        }
        
        
    }
}
