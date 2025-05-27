using System;
using UnityEngine;

namespace StateMachine
{
    public interface IEntity
    {
       
       
    }
    public class Entity : ComponentBehavior, IEntity
    {
        public StateMachine StateMachine;

        [Header("State")] 
        public IdleState IdleState;
        public UseCardState UseCardState;
        public RunState RunState;
       
        
        [Header("Health")] 
        public int MaxHP;
        public int CurHP;
        public int Armor;
        
        [Header("Use Card")] 
        
        public bool MustReachTarget;
        public CardStrategy CardStrategy;
        public Action OnFinishedUsingCard;
      
        
        [Header("Attack")] 
        public float AttackRange;
        public int Damage;
        public Transform EnemyTarget;

        [Header("Move")] 
        public bool IsRunningToTarget;
        public float RunSpeed;
        public Vector3 StandPoint;
       
        public bool IsOriginalFacingRight;

        [Header("Componnet")] 
        public Animator Anim;

        public bool IsAnimationTriggerFinished;
        
        private Transform model;
        protected override void Awake()
        {
            base.Awake();
            IdleState = new IdleState(this, "Idle");
            RunState = new RunState(this, "Run");
            UseCardState = new UseCardState(this, String.Empty);

            StandPoint = transform.position;
            MustReachTarget = false;
            
            
            StateMachine = new StateMachine();
            Any(IdleState, new FuncPredicate(() => GameManager.Instance.IsTurn(GameStateType.CollectingCard)));
            Any(IdleState, new FuncPredicate(() => GameManager.Instance.IsTurn(GameStateType.DistributeCard)));

          
        }

        protected virtual void Update() => StateMachine.Update();

        protected void FixedUpdate() => StateMachine.FixedUpdate();
        
        protected void At(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddTransition(from, to, condition, getData);
        protected void Any(IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddAnyTransition(to, condition, getData);

       
    
        protected override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponent<Animator>();
            if (model == null) model = transform.Find("Model");
        }
        public void SetFacing(bool isFacingRight = true)
        {
            Vector3 curLocalScale = model.localScale; 
            model.localScale = new Vector3((isFacingRight ? 1 : -1) * Mathf.Abs(curLocalScale.x), curLocalScale.y, curLocalScale.z);
        }
        protected void AnimationTrigger() => StateMachine.State.AnimationTrigger();
        protected void AnimationFinishTrigger() => StateMachine.State.AnimationFinishTrigger();

     
       
       
       
        
        
    }
}