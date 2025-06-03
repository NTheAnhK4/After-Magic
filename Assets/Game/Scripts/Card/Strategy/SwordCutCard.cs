

using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Sword Cut Card", fileName = "Sword Cut Card")]
public class SwordCutCard : CardStrategy
{
    public int BaseDamage;
    public int DamageRate;
    public int Level = 1;
  
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

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (_enemy != null) _enemy.TakeDamage(GetDamage());
    }

    private int GetDamage()
    {
        return _owner.Damage + BaseDamage + DamageRate * Level;
    }
}
