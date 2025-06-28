using System;
using BrokerChain.Status;



namespace BrokerChain
{
    public class EntityStatModifier : StatModifier
    {
        private readonly StatsType type;
        private readonly Func<int, int> operation;
       

        public override void Handle(object sender, Query query)
        {
            if (query.StatsType == type) query.Value = operation(query.Value);
        }


        public EntityStatModifier( int turnApplyValue, ExpireTiming expireTiming, StatsType type, Func<int,int> operation) : base( turnApplyValue, expireTiming)
        {
            this.type = type;
            this.operation = operation;
        }
        
    }
}