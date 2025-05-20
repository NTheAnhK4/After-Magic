using System;

using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : Entity
{
    #region Player States

    public PlayerData Data;
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }

    #endregion

    #region Checker

    public bool IsAnimationTriggerFinished;

    public bool IsRunningToEnemy;

    #endregion

    public Transform EnemyTarget;
    public Animator Anim { get; private set; }
    public Rigidbody2D RB { get; private set; }
    private Vector2 standPositon;
    private Transform model;
   

    
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (Anim == null) Anim = GetComponent<Animator>();
        if (RB == null) RB = GetComponent<Rigidbody2D>();
        if (model == null) model = transform.Find("Model");
    }

    protected override void Awake()
    {
        base.Awake();
        IsRunningToEnemy = false;
     
        IdleState = new PlayerIdleState(this, "Idle");
        RunState = new PlayerRunState(this, "Run");
        AttackState = new PlayerAttackState(this, "Attack");
        standPositon = transform.position;
        
        Any(RunState, new FuncPredicate(RunToTargetCondition()),
            () => new PlayerRunStataData { TargetPosition = EnemyTarget.position, IsRunningToEnemy = true });
        At(RunState, AttackState, new FuncPredicate(EnemyInAttackRange()));
        At(AttackState, RunState, new FuncPredicate(RunToStandPositionCondition()),
            () => new PlayerRunStataData { TargetPosition = standPositon, IsRunningToEnemy = false });
        At(RunState, IdleState, new FuncPredicate(BackToStandPosition()));

        StateMachine.SetState(IdleState);
    }

    protected override void Update()
    {
        
        if (!GameManager.Instance.IsTurn(GameStateType.UsingCard)) return;
        base.Update();
    }

    Func<bool> EnemyInAttackRange() =>
        () =>IsRunningToEnemy && !IsAnimationTriggerFinished &&
              EnemyTarget != null &&
              Vector2.Distance(EnemyTarget.position, transform.position) <= Data.AttackRange;

  

    Func<bool> RunToTargetCondition() =>
        () => EnemyTarget != null &&
              Vector2.Distance(EnemyTarget.position, transform.position) > Data.AttackRange;

    Func<bool> RunToStandPositionCondition() =>
        () => IsAnimationTriggerFinished ;

    Func<bool> BackToStandPosition() =>
        () => !IsAnimationTriggerFinished && !IsRunningToEnemy &&
              Vector2.Distance(transform.position, standPositon) < 0.1f;

  
    private void AnimationTrigger() => ((PlayerState)(StateMachine.State)).AnimationTrigger();
    private void AnimationFinishTrigger() => ((PlayerState)(StateMachine.State)).AnimationFinishTrigger();

    public void SetFacing(bool isFacingRight = true)
    {
        Vector3 curLocalScale = model.localScale; 
        model.localScale = new Vector3((isFacingRight ? 1 : -1) * Mathf.Abs(curLocalScale.x), curLocalScale.y, curLocalScale.z);
    }
}
