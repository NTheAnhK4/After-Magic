using System;
using AudioSystem;
using BrokerChain;
using DG.Tweening;
using UnityEngine;
namespace StateMachine
{
    public class Entity : ComponentBehaviour, IEntity
    {
        public bool IsSpriteFacingRight;
        public string curentState;
        public StateMachine StateMachine;
        public StatsSystem StatsSystem;
        public DamagePopupUI damagePopupUI;
       
        [Header("State")] 
        public IdleState IdleState;
        public UseCardState UseCardState;
        public RunState RunState;
        public HurtState HurtState;
        public DeadState DeadState;

      
       

    
        public bool IsHurting { get; set; }
        public Action OnDead;
       
        
        public bool MustReachTarget { get; set; }
        public CardStrategy CardStrategy { get; set; }
        public Action OnFinishedUsingCard;
      
        
        
        public float AttackRange { get; set; }
        
        public virtual Entity EnemyTarget { get; set; }

       
        public bool IsRunningToTarget { get; set; }
        public float RunSpeed { get; set; }
        public Vector3 StandPoint { get; set; }
       
        public bool IsOriginalFacingRight { get; set; }
        public bool IsDead { get; set; }
     
        public Animator Anim { get; set; }

        public bool IsAnimationTriggerFinished { get; set; }
        
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
            RunSpeed = 20;
        }

        protected virtual void Update() => StateMachine.Update();

        protected void FixedUpdate() => StateMachine.FixedUpdate();

        #endregion

        #region State Machine Functions

        protected void At(IState from, IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddTransition(from, to, condition, getData);
        protected void Any(IState to, IPredicate condition, Func<StateData> getData = null) => StateMachine.AddAnyTransition(to, condition, getData);

        
        #endregion
       
        
      
       
    
        public override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponentInChildren<Animator>();
            if (model == null) model = transform.Find("Model");
            if (StatsSystem == null) StatsSystem = GetComponent<StatsSystem>();
            if (damagePopupUI == null) damagePopupUI = transform.Find("UI").GetComponentInChildren<DamagePopupUI>();
            damagePopupUI.OrNull()?.gameObject.SetActive(false);
        }
        public void SetFacing(bool isFacingRight = true)
        {
            Vector3 curLocalScale = model.localScale;
            if (IsSpriteFacingRight) model.localScale = curLocalScale.With(x: (isFacingRight ? 1 : -1) * Mathf.Abs(curLocalScale.x));
            else model.localScale = curLocalScale.With(x: (isFacingRight ? -1 : 1) * Mathf.Abs(curLocalScale.x));
            
        }
       
       
        protected void StartTurn(object param) => StatsSystem.StartTurn();
        protected void EndTurn(object param) => StatsSystem.EndTurn();


    }
}