using System;
using BrokerChain;
using DG.Tweening;
using UnityEngine;
namespace StateMachine
{
    public class Entity : ComponentBehavior, IEntity
    {
        
        public StateMachine StateMachine;
        public StatsSystem StatsSystem;

       
        [Header("State")] 
        public IdleState IdleState;
        public UseCardState UseCardState;
        public RunState RunState;
        public HurtState HurtState;
        public DeadState DeadState;
       
        
       
       

    
        public bool IsHurting;
        public Action OnDead;
        [Header("Use Card")] 
        
        public bool MustReachTarget;
        public CardStrategy CardStrategy;
        public Action OnFinishedUsingCard;
      
        
        [Header("Attack")] 
        public float AttackRange;
        
        public virtual Entity EnemyTarget { get; set; }

        [Header("Move")] 
        public bool IsRunningToTarget;
        public float RunSpeed;
        public Vector3 StandPoint;
       
        public bool IsOriginalFacingRight;

        [HideInInspector] public bool IsDead;
        [Header("Componnet")] 
        public Animator Anim;

        public bool IsAnimationTriggerFinished;
        
        private Transform model;

     
        #region Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            IdleState = new IdleState(this, "Idle");
            RunState = new RunState(this, "Run");
            HurtState = new HurtState(this, "Hurt");
            DeadState = new DeadState(this, "Dead");
            UseCardState = new UseCardState(this, String.Empty);

           
            StateMachine = new StateMachine();
            Any(DeadState, new FuncPredicate(() => IsDead));
            Any(HurtState, new FuncPredicate(() => IsHurting));
            Any(IdleState, new FuncPredicate(() => InGameManager.Instance.IsTurn(GameStateType.CollectingCard)));
            Any(IdleState, new FuncPredicate(() => InGameManager.Instance.IsTurn(GameStateType.DistributeCard)));

          
        }

        protected virtual void OnEnable()
        {
            StandPoint = transform.position;
            MustReachTarget = false;
            
            IsRunningToTarget = false;
            IsHurting = false;
            IsDead = false;
        }

        protected virtual void Update() => StateMachine.Update();

        protected void FixedUpdate() => StateMachine.FixedUpdate();

        #endregion

        #region State Machine Functions

        protected void At(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddTransition(from, to, condition, getData);
        protected void Any(IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddAnyTransition(to, condition, getData);

        protected void AnimationTrigger() => StateMachine.State.AnimationTrigger();
        protected void AnimationFinishTrigger() => StateMachine.State.AnimationFinishTrigger();

        #endregion
       
        
      
       
    
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponent<Animator>();
            if (model == null) model = transform.Find("Model");
            if (StatsSystem == null) StatsSystem = GetComponent<StatsSystem>();
        }
        public void SetFacing(bool isFacingRight = true)
        {
            Vector3 curLocalScale = model.localScale; 
            model.localScale = new Vector3((isFacingRight ? 1 : -1) * Mathf.Abs(curLocalScale.x), curLocalScale.y, curLocalScale.z);
        }
       
       
        protected void StartTurn(object param) => StatsSystem.StartTurn();
        protected void EndTurn(object param) => StatsSystem.EndTurn();


    }
}