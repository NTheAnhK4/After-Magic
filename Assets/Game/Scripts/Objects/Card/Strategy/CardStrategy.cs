
using BrokerChain.Status;
using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class CardStrategy : ScriptableObject
{
    public int CardLevel = 1;
    public string AnimName;
    public bool MustReachTarget;
    public bool AppliesToAlly;
    
    protected Entity _owner;
    protected Entity _enemy;

    public virtual void Apply(Entity owner, Entity enemy)
    {
        this._owner = owner;
        this._enemy = enemy;
    }
   

    public abstract void Remove();
    public abstract bool HasFinisedUsingCard();

    public virtual void AnimationTrigger()
    {
        
    }
}
