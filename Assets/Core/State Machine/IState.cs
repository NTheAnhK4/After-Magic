

using System;

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
        private Func<string> animNameFunc;

        public State(T entity, string animBoolName)
        {
            this.entity = entity;
            this.animBoolName = animBoolName;
        }

        public State(T entity, Func<string> animNameFunc)
        {
            this.entity = entity;
            this.animNameFunc = animNameFunc;
        }

       

        public virtual void OnEnter(StateData stateData = null)
        {
            if (animNameFunc != null)   animBoolName = animNameFunc.Invoke();
            if(!string.IsNullOrEmpty(animBoolName)) entity.Anim.SetBool(animBoolName, true);
         
            entity.IsAnimationTriggerFinished = false;
            entity.curentState = this.GetType().Name;
            // Debug.Log(entity.name + " " + this.GetType().Name);
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {
            
        }

        public virtual void OnExit()
        { 
            if(!string.IsNullOrEmpty(animBoolName)) entity.Anim.SetBool(animBoolName, false);
        }

        public virtual void AnimationFinishTrigger() => entity.IsAnimationTriggerFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
    
}

