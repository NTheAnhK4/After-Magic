using System;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

namespace StateMachine
{
    public class StateMachine
    {
        class StateNode
        {
            public IState State { get; }
         
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                State = state;
                Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition, Func<StateData> getstateData = null)
            {
                Transitions.Add(new Transition(to, condition, getstateData));
            }
            
        }

        private StateNode current;
        public IState State => current.State;
        private Dictionary<Type, StateNode> nodes = new();
        private HashSet<ITransition> anyTransitions = new();

        public void Update()
        {
            var transition = GetTransition();
            if (transition != null) ChangeState(transition.To, transition.Data);
           
            
            current.State?.Update();
        }

        public void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }
        //use for specific condition
        public void SetState(IState state, Func<StateData> stateData = null)
        {
            
            current = nodes[state.GetType()];
            current.State?.OnEnter(stateData?.Invoke());
        }

        public void ChangeState(IState state, Func<StateData> stateData = null)
        {
            if (state == current.State) return;

            var previousState = current.State;
            var nextState = nodes[state.GetType()].State;
            
            previousState?.OnExit();
            
            nextState?.OnEnter(stateData?.Invoke());
            current = nodes[state.GetType()];
        }

        ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
            {
                if (transition.Condition.Evaluate()) return transition;
            }

          
            foreach (var transition in current.Transitions)
            {
                if (transition.Condition.Evaluate()) return transition;
            }

            return null;
        }

        public void AddTransition(IState from, IState to, IPredicate condition, Func<StateData> getData = null)
        {
            GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition, getData);
        }

        public void AddAnyTransition(IState to, IPredicate condition, Func<StateData> getData = null)
        {
            anyTransitions.Add(new Transition(GetOrAddNode(to).State, condition, getData));
        }
        StateNode GetOrAddNode(IState state)
        {
            var node = nodes.GetValueOrDefault(state.GetType());
            if (node == null)
            {
                node = new StateNode(state);
                nodes.Add(state.GetType(),node);
            }

            return node;
        }
        
    }
    
    
}
