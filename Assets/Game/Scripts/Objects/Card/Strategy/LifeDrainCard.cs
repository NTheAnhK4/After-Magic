using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Life Drain Card", fileName = "Life Drain Card")]
public class LifeDrainCard : CardStrategy
{
    public int BaseDamage = 3;
    public int DamageRate = 1;
  
    public override void Remove()
    {
        
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (_enemy != null)
        {
            int value = _enemy.StatsSystem.TakeDamage(GetDamage());
            if(_owner != null && _owner.StatsSystem != null) _owner.StatsSystem.Health(value);
        }
    }

    public override bool HasFinisedUsingCard()
    {
        return _owner.IsAnimationTriggerFinished;
    }
    private int GetDamage()
    {
        return _owner.StatsSystem.Stats.Damage + BaseDamage + DamageRate * CardLevel;
    }
}
