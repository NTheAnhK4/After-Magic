using System;
using System.Collections.Generic;
using BrokerChain.Status;


namespace BrokerChain
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers = new();
       
        public event EventHandler<Query> Queries;
        public void PerformQuery(object sender, Query query) => Queries?.Invoke(sender, query);

        public void AddModifier(StatusEffectData statusEffectData, Action onRemoved = null)
        {
           
            StatModifier modifier = statusEffectData.GetEffect();
         
            
            modifier.OnRemoved = onRemoved;
            modifiers.AddLast(modifier);
            Queries += modifier.Handle;
            modifier.OnDispose += _ =>
            {
                modifiers.Remove(modifier);
                Queries -= modifier.Handle;
            };
        }
        
        public void Update(ExpireTiming expireTiming)
        {
            var node = modifiers.First;
            //Update all modifier
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(expireTiming);
                node = node.Next;
            }
            //Dispose any that are finished
            node = modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next;
                if(node.Value.MarkedForRemoval) node.Value.Dispose();
                node = nextNode;
            }
        }

    }
}