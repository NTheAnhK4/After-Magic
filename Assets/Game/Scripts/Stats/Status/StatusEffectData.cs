using System;
using UnityEngine;


namespace BrokerChain.Status
{
    public enum ExpireTiming
    {
        EndOfThisTurn,
        StartOfThisTurn
    }

    public enum StatusEffectType
    {
        Poisoned,
        DecreaseDamage,
        IncreaseDamage,
        Shield
    }

  
    [CreateAssetMenu(menuName = "Data/Status Effect Data", fileName = "Status Effect Data")]
    public class StatusEffectData : ScriptableObject
    {
        public StatusEffectType StatusEffectType;
        public Sprite Icon;
        public int TurnApply;
       
        public int Value;
       
        
        public StatsType Type;
        
        
        public ExpireTiming ExpireTiming;

        public OperatorType OperatorType;

        public StatModifier GetEffect()
        {
            return OperatorType switch
            {
                OperatorType.Add => new EntityStatModifier(TurnApply, ExpireTiming, Type, v => v + Value),
                OperatorType.Multiply => new EntityStatModifier(TurnApply, ExpireTiming, Type, v => v * Value),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
        public StatusEffectData Clone()
        {
            StatusEffectData clone = CreateInstance<StatusEffectData>();
            clone.StatusEffectType = this.StatusEffectType;
            
            clone.Icon = this.Icon;
            clone.TurnApply = this.TurnApply;
            clone.Value = this.Value;
            clone.Type = this.Type;
            clone.ExpireTiming = this.ExpireTiming;
            clone.OperatorType = this.OperatorType;
            return clone;
        }

    }
}