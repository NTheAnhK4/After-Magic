using System;
using UnityEngine;

namespace BrokerChain
{
   
    
    public class EntityStats
    {
        public int MaxHP;       
                
        public int Defense;     
        public int Damage;
        private int hp;

        public int HP
        {
            get => hp;
            set{
                if (hp != value)
                {
                    hp = value;
                    OnHPChange?.Invoke(hp, MaxHP);
                }
            }
        }
        public Action<int,int> OnHPChange; 
    }

    // Wrapper around EntityStats that allows dynamic value modification via a StatsMediator (Chain of Responsibility pattern)
   
    public class Stats
    {
        private readonly StatsMediator mediator; // Mediator responsible for modifying stat queries dynamically
        public readonly EntityStats EntityStats; // The base stats data

        // Property to expose the mediator (read-only)
        public StatsMediator Mediator => mediator;

        // Computed Damage property: can be modified by the mediator
        public int Damage
        {
            get
            {
                var q = new Query(StatsType.Damage, EntityStats.Damage);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        // Computed Defense property: can be modified by the mediator
        public int Defense
        {
            get
            {
                var q = new Query(StatsType.Defense, EntityStats.Defense);
                mediator.PerformQuery(this, q);
                return q.Value;
            }
        }

        

        // Constructor: initializes the Stats object with a mediator and base stats
        public Stats(StatsMediator mediator, EntityStats entityStats)
        {
            this.mediator = mediator;
            this.EntityStats = entityStats;
        }
    }
}
