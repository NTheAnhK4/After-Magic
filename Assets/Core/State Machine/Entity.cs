using System;
using UnityEngine;

namespace StateMachine
{
    public class Entity : ComponentBehavior
    {
        public StateMachine StateMachine;

        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine();
        }

        protected virtual void Update() => StateMachine.Update();

        protected void FixedUpdate() => StateMachine.FixedUpdate();
        
        protected void At(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddTransition(from, to, condition, getData);
        protected void Any(IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddAnyTransition(to, condition, getData);

    }
}