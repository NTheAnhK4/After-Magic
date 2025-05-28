using System;
using DG.Tweening;
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
        public HurtState HurtState;
        public DeadState DeadState;
       
        
        [Header("Health")] 
        public int MaxHP;
        public int CurHP;
        public int Armor;
        public bool IsHurting;
        public Action OnDead;
        [Header("Use Card")] 
        
        public bool MustReachTarget;
        public CardStrategy CardStrategy;
        public Action OnFinishedUsingCard;
      
        
        [Header("Attack")] 
        public float AttackRange;
        public int Damage;
        public Entity EnemyTarget;

        [Header("Move")] 
        public bool IsRunningToTarget;
        public float RunSpeed;
        public Vector3 StandPoint;
       
        public bool IsOriginalFacingRight;

        [Header("Componnet")] 
        public Animator Anim;

        public bool IsAnimationTriggerFinished;
        
        private Transform model;

        #region Selectable Value

        private SpriteRenderer aim;
        private Color originalColor;
        private Vector3 originalScale;
        private Tween tween;

        #endregion

        #region Unity Callback Functions

        protected override void Awake()
        {
            base.Awake();
            IdleState = new IdleState(this, "Idle");
            RunState = new RunState(this, "Run");
            HurtState = new HurtState(this, "Hurt");
            DeadState = new DeadState(this, "Dead");
            UseCardState = new UseCardState(this, String.Empty);

            StandPoint = transform.position;
            MustReachTarget = false;
            
            IsRunningToTarget = false;
            IsHurting = false;
            StateMachine = new StateMachine();
            Any(DeadState, new FuncPredicate(() => CurHP <= 0));
            Any(HurtState, new FuncPredicate(() => IsHurting));
            Any(IdleState, new FuncPredicate(() => GameManager.Instance.IsTurn(GameStateType.CollectingCard)));
            Any(IdleState, new FuncPredicate(() => GameManager.Instance.IsTurn(GameStateType.DistributeCard)));

          
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
       
        
      
       
    
        protected override void LoadComponent()
        {
            base.LoadComponent();
            if (Anim == null) Anim = GetComponent<Animator>();
            if (model == null) model = transform.Find("Model");
            if (aim == null)
            {
                aim = transform.Find("Aim").GetComponent<SpriteRenderer>();
                originalColor = aim.color;
                aim.gameObject.SetActive(false);
                originalScale = aim.transform.localScale;
            }
        }
        public void SetFacing(bool isFacingRight = true)
        {
            Vector3 curLocalScale = model.localScale; 
            model.localScale = new Vector3((isFacingRight ? 1 : -1) * Mathf.Abs(curLocalScale.x), curLocalScale.y, curLocalScale.z);
        }
       
        public void TakeDamage(int damage)
        {
            if (Armor > 0)
            {
                damage = Math.Max(0, damage - Armor);
                Armor = Math.Max(0, Armor - damage);
            }

            if (damage > 0) IsHurting = true;
            CurHP = Math.Max(0, CurHP - damage);
        }

        #region Selectable

       

        protected void ToggleAim(bool isSetActive)
        {
            aim.color = originalColor;
            aim.gameObject.SetActive(isSetActive);
        }
        public void SelectObject()
        {
            aim.color = Color.red;
            aim.transform.localScale = originalScale * 1.2f; 
            tween = aim.transform.DOScale(originalScale * 1.5f, .5f)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);
        }

        public void DeselectObject()
        {
            tween?.Kill();
            aim.transform.localScale = originalScale;
            aim.color = originalColor;
        }

        #endregion
        
    }
}