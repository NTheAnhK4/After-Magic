

using BrokerChain.Status;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Card Strategy/Poisoned Sword Card", fileName = "Poisoned Sword Card")]
public class PoisonedSwordCard : CardStrategy
{
    public StatusEffectData StatusEffectData;

    [Header("Poinsoned Effect")] 
    public int TurnApply;

    public float TurnApplyRate;

    public int AddedDamage;

    public int AddedDamageRate;
    [Header("Damage")]
    public int BaseDamage;
    public int DamageRate;
    public override void Remove()
    {
        
    }

    public override bool HasFinisedUsingCard()
    {
        if (_owner == null) return true;
        return _owner.IsAnimationTriggerFinished;
    }
    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
        if (_enemy != null)
        {
            _enemy.StatsSystem.TakeDamage(GetDamage());
            if (StatusEffectData != null)
            {
                StatusEffectData.TurnApply = GetTurnApply();
                StatusEffectData.Value = GetAddedDamage();
                _enemy.StatsSystem.AddModifier(StatusEffectData.Clone());
            }
        }
    }
    private int GetDamage()
    {
        return _owner.StatsSystem.Stats.Damage + BaseDamage + DamageRate * CardLevel;
    }

    private int GetTurnApply()
    {
        return TurnApply + Mathf.FloorToInt(CardLevel * TurnApplyRate);
    }

    private int GetAddedDamage()
    {
        return AddedDamage + BaseDamage * CardLevel;
    }
}
