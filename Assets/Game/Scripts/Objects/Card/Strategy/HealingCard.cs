
using StateMachine;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Healing Card", fileName = "Healing Card")]

public class HealingCard : CardStrategy
{
    public int healedAmount;
    public int healedAmountRate;
  
    public override void Apply(Entity owner, Entity enemy)
    {
        base.Apply(owner, enemy);
        if (owner != null)
        {
            if (owner.StatsSystem != null) owner.StatsSystem.Health(GetHealedAmount());
           
        }
    }

    public override void Remove()
    {
        
    }

    public override bool HasFinisedUsingCard()
    {
        return _owner.IsAnimationTriggerFinished;
    }


    private int GetHealedAmount() => healedAmount + CardLevel * healedAmountRate;
}
