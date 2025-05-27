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
        void AnimationFinishTrigger();
        void AnimationTrigger();

    }

    public class State<T> : IState where T : Entity
    {
        protected T entity;
        private string animBoolName;

        public State(T entity, string animBoolName)
        {
            this.entity = entity;
            this.animBoolName = animBoolName;
        }

       

        public virtual void OnEnter(StateData stateData = null)
        {
            if(animBoolName != string.Empty) entity.Anim.SetBool(animBoolName, true);
         
            entity.IsAnimationTriggerFinished = false;
          //  Debug.Log(entity.name + " " + this.GetType().Name);
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void OnExit()
        {
            if(animBoolName != string.Empty) entity.Anim.SetBool(animBoolName, false);
        }

        public virtual void AnimationFinishTrigger() => entity.IsAnimationTriggerFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
    
}

