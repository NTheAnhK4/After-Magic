
using BrokerChain.Status;
using StateMachine;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Drain Strength Card", fileName = "Drain Strength Card")]
public class DrainStrengthCard : CardStrategy
{
    public StatusEffectData damageEffectData;
    [Header("Damage Decrease")]
    public Sprite DecreaseIcon;
    public int DamageDecrease = 1;
    public int DamageDecreaseRate = 1;
    public int DecreaseTurnApply = 2;
    [Header("Damage Increase")] 
    public Sprite IncreaseIcon;
    public int DamageIncrease = 1;
    public int DamageIncreaseRate = 1;
    public int IncreaseTurnApply = 2;
    
    public override void Remove()
    {
        
    }

    public override void Apply(Entity owner, Entity enemy)
    {
        base.Apply(owner, enemy);
        if (_owner != null && _owner.StatsSystem != null)
        {
            StatusEffectData increaseDamageEffect = damageEffectData.Clone();
            increaseDamageEffect.TurnApply = IncreaseTurnApply;
            increaseDamageEffect.Value = GetDamageIncrease();
            
            
            increaseDamageEffect.Icon = IncreaseIcon;
            increaseDamageEffect.Name = "Increase damage effect";
            _owner.StatsSystem.AddModifier(increaseDamageEffect);
        }

        if (_enemy != null && _enemy.StatsSystem != null)
        {
            StatusEffectData decreaseDamageEffect = damageEffectData.Clone();
            
            decreaseDamageEffect.TurnApply = DecreaseTurnApply;
            decreaseDamageEffect.Value = -1 * GetDamageDecrease();
            decreaseDamageEffect.Icon = DecreaseIcon;
            decreaseDamageEffect.Name = "Decrease damage effect";
            _enemy.StatsSystem.AddModifier(decreaseDamageEffect);
        }
    }


    public override bool HasFinisedUsingCard()
    {
        return _owner.IsAnimationTriggerFinished;
    }

    public int GetDamageDecrease() => DamageDecrease + CardLevel * DamageDecreaseRate;
    public int GetDamageIncrease() => DamageIncrease + CardLevel * DamageIncreaseRate;
}
