using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    public class StateData
    {
        
    }
    public interface IState
    {
        void OnEnter(StateData stateData = null);
        void Update();
        void FixedUpdate();
        void OnExit();
    }

    public class State<T> : IState where T : Entity
    {
        protected T entity;

        public State(T entity)
        {
            this.entity = entity;
        }
        public virtual void OnEnter(StateData stateData = null)
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            
        }
    }
    
}

