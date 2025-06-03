
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;

public class Enemy : Entity
{
   
    #region State
    
    public EnemyPlanningState PlanningState { get; set; }

    #endregion

    #region Planning

    public List<EnemyCardData> CardStrategyAvailables;
    public bool IsPlanningState { get; set; }
    public bool CanUseCard { get; set; }
    public EnemyActionType PredictedAction { get; set; }
    public Transform PredictedActionTrf { get; set; }
   
    #endregion
   
    protected override void Awake()
    {
        base.Awake();
        CanUseCard = false;
        
       
       
        RunSpeed = 10f;
        
        AttackRange = 2.5f;
        Damage = 1;
        MaxHP = 20;
        CurHP = 1;
        Armor = 0;
        
        IsOriginalFacingRight = false;

        OnFinishedUsingCard += () => CanUseCard = false;
        PlanningState = new EnemyPlanningState(this, "Idle");
        
        Any(PlanningState, new FuncPredicate(InPlanningState()), () => new EnemyPlanningStateData(){Player = PlayerPartyManager.Instance.GetPlayer()});
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.transform.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    

       
        StateMachine.SetState(IdleState);
    }
    

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (PredictedActionTrf == null) PredictedActionTrf = transform.Find("UI").Find("NextAction");
    }

    private void Start()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, param => IsPlanningState = true);
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, param => IsPlanningState = false);
       
    }

    private void OnEnable()
    {
        if(EnemyManager.Instance != null) EnemyManager.Instance.AddEnemy(this);
        
    }
            
    private void OnDisable()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }
    

    public async UniTask DoAction()
    {
        CanUseCard = true;
        await UniTask.WaitUntil(() => CanUseCard== false);
    }
  
    Func<bool> RunToTargetCondition() => () =>CanUseCard && CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.transform.position, transform.position) > AttackRange;
    
    Func<bool> InPlanningState() => () => IsPlanningState && !IsHurting;
    Func<bool> CanEnterUseCardState() => () =>!IsPlanningState && CanUseCard && CardStrategy != null && !MustReachTarget;

}
