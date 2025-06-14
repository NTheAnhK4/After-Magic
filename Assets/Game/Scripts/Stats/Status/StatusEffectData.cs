using System;
using UnityEngine;


namespace BrokerChain.Status
{
    [CreateAssetMenu(menuName = "Data/Status Effect Data", fileName = "Status Effect Data")]
    public class StatusEffectData : ScriptableObject
    {
        public string Name;
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
    }
}