using System;
using Develop.Utilities;
using UnityEngine;

namespace Develop
{
    public abstract class StatModifier : IDisposable
    {
        public readonly Sprite icon;
        public bool MarkedForRemoval { get; private set; }
        public event Action<StatModifier> OnDispose = delegate {  };

        private readonly CountdownTimer timer;
        public abstract void Handle(object sender, Query query);

        protected StatModifier(Sprite icon,  float duration)
        {
            this.icon = icon;
            if (duration <= 0) return;
            timer = new CountdownTimer(duration);
            timer.OnTimerStop += () => MarkedForRemoval = true;
            timer.Start();
        }

        public void Update(float deltaTime) => timer?.Tick(deltaTime);

        public void Dispose()
        {
            OnDispose.Invoke(this);
        }
    }

    public class BasicStatModifier : StatModifier
    {
        private readonly StatsType type;
        private readonly Func<int, int> operation;

      

        public override void Handle(object sender, Query query)
        {
            if (query.StatsType == type)
            {
                query.Value = operation(query.Value);
            }
        }

        public BasicStatModifier(Sprite icon, float duration, StatsType type, Func<int,int> operation) : base(icon, duration)
        {
            this.type = type;
            this.operation = operation;
        }
    }
}