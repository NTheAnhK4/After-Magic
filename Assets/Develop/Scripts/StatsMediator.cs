using System;
using System.Collections.Generic;

namespace Develop
{
    public class StatsMediator
    {
        private readonly LinkedList<StatModifier> modifiers = new();

        public event EventHandler<Query> Queries;
        public void PerformQery(object sender, Query query) => Queries?.Invoke(sender, query);

        public void AddModifier(StatModifier modifier)
        {
            modifiers.AddLast(modifier);
            Queries += modifier.Handle;

            modifier.OnDispose += _ =>
            {
                modifiers.Remove(modifier);
                Queries -= modifier.Handle;
            };
        }

        public void Update(float deltaTime)
        {
            //Update all modifier will deltatime
            var node = modifiers.First;
            while (node != null)
            {
                var modifier = node.Value;
                modifier.Update(deltaTime);
                node = node.Next;
            }
            // Dispose any that are finished, a.k.a Mark and Sweep

            node = modifiers.First;
            while (node != null)
            {
                var nextNode = node.Next;
                if (node.Value.MarkedForRemoval) node.Value.Dispose();

                node = nextNode;
            }
        }
    }

    public class Query
    {
        public readonly StatsType StatsType;
        public int Value;

        public Query(StatsType statsType, int value)
        {
            this.StatsType = statsType;
            this.Value = value; 
        }
    }
}