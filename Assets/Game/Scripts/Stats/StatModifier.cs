using System;
using BrokerChain.Status;
using UnityEngine;
using Utilities;

namespace BrokerChain
{
    public abstract class StatModifier : IDisposable
    {
      
        private readonly ExpireTiming ExpireTiming;
        public bool MarkedForRemoval { get; private set; }

        public event Action<StatModifier> OnDispose = delegate { };
        
        private readonly CountdownTimer timer;

        public Action OnRemoved;
        public abstract void Handle(object sender, Query query);
        
        protected StatModifier(int turnApplyValue, ExpireTiming expireTiming)
        {
           
            this.ExpireTiming = expireTiming;
            
            if (turnApplyValue <= 0) return;
            timer = new CountdownTimer(turnApplyValue);

            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
        }

        public void Update(ExpireTiming expireTiming)
        {
            if (expireTiming != ExpireTiming) return;
            timer.Tick(1);
        }

        public void Dispose()
        {
            OnRemoved?.Invoke();
            OnDispose.Invoke(this);
        }
    }
}
