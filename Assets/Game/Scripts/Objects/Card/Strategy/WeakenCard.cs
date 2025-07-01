
using BrokerChain.Status;

using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Weaken Card", fileName = "Weaken Card")]
public class WeakenCard : CardStrategy
{
    public StatusEffectData damageEffectData;
    [Header("Damage Decrease")]
    public Sprite DecreaseIcon;
    public int DamageDecrease = 1;
    public int DamageDecreaseRate = 1;
    public int DecreaseTurnApply = 2;
    [Header("Damage")] 
   
    public int Damage = 1;
    public int DamageRate = 1;
    
 
    public override void Remove()
    {
        
    }


    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (_enemy != null && _enemy.StatsSystem != null)
        {
            _enemy.StatsSystem.TakeDamage(GetDamage());
            StatusEffectData decreaseDamageEffect = damageEffectData.Clone();
            decreaseDamageEffect.TurnApply = DecreaseTurnApply;
            decreaseDamageEffect.Value = -1 * GetDamageDecrease();
            decreaseDamageEffect.Icon = DecreaseIcon;
            decreaseDamageEffect.StatusEffectType = StatusEffectType.DecreaseDamage;
            
            _enemy.StatsSystem.AddModifier(decreaseDamageEffect);

           
        }
    }

    public override bool HasFinisedUsingCard()
    {
        return _owner.IsAnimationTriggerFinished;
    }

    public int GetDamageDecrease() => DamageDecrease + CardLevel * DamageDecreaseRate;
    public int GetDamage()
    {
        int damage = Damage + CardLevel * DamageRate;
        if (_owner != null && _owner.StatsSystem != null) damage += _owner.StatsSystem.Stats.Damage;
        return damage;
    }
}
