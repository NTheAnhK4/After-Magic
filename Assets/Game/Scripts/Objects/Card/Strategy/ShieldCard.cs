
using BrokerChain.Status;
using StateMachine;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Shield Card", fileName = "Shield Card")]
public class ShieldCard : CardStrategy
{
    public StatusEffectData StatusEffectData;
    public override void Apply(Entity owner, Entity enemy)
    {
        base.Apply(owner, enemy);
        if (owner != null && StatusEffectData != null) owner.StatsSystem.AddModifier(StatusEffectData.Clone());
        
    }

    public override void Remove()
    {
        
    }
    public override bool HasFinisedUsingCard()
    {
        
        return  _owner.IsAnimationTriggerFinished;
    }

  
}
