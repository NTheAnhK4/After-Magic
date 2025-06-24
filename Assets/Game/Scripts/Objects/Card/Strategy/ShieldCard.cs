
using StateMachine;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Shield Card", fileName = "Shield Card")]
public class ShieldCard : CardStrategy
{
  

    public override void Execute()
    {
        
    }

    public override void Remove()
    {
        
    }
    public override bool HasFinisedUsingCard()
    {
        
        return base.HasFinisedUsingCard() && _owner.IsAnimationTriggerFinished;
    }

  
}
