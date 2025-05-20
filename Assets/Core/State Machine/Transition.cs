using System;

namespace StateMachine
{
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        public StateData Data => getData?.Invoke();
        private readonly Func<StateData> getData;

        public Transition(IState to, IPredicate condition, Func<StateData> getData = null)
        {
            To = to;
            Condition = condition;
            this.getData = getData;
        }
    }
}