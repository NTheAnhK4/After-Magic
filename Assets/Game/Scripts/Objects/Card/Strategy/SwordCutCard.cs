

using UnityEngine;


[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Sword Cut Card", fileName = "Sword Cut Card")]
public class SwordCutCard : CardStrategy
{
    public int BaseDamage;
    public int DamageRate;
   
  
   
    public override void Remove()
    {
        
    }

    public override bool HasFinisedUsingCard()
    {
        return _owner.IsAnimationTriggerFinished;
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (_enemy != null) _enemy.StatsSystem.TakeDamage(GetDamage());
    }

    private int GetDamage()
    {
        return _owner.StatsSystem.Stats.Damage + BaseDamage + DamageRate * CardLevel;
    }
}
