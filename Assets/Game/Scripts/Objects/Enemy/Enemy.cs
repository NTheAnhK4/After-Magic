
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using StateMachine;

using UnityEngine;

public class Enemy : Entity
{
  
    [SerializeField] private OscillatingMovement oscillatingMovement;
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

    #region Action

    private Action<object> onPlayerTurnAction;
    private Action<object> onEnemyTurnAction;

    #endregion
    public override Entity EnemyTarget
    {
        get => PlayerPartyManager.Instance.GetRandomPartyMember(); 
        
    }

    protected override void Awake()
    {
        base.Awake();
       
        PlanningState = new EnemyPlanningState(this, "Idle");
        
        Any(PlanningState, new FuncPredicate(InPlanningState()), () => new EnemyPlanningStateData(){Player = PlayerPartyManager.Instance.GetPlayer()});
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.transform.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    

       
       
    }
    

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (PredictedActionTrf == null) PredictedActionTrf = transform.Find("UI").Find("NextAction");
        if (oscillatingMovement == null) oscillatingMovement = transform.Find("UI").GetComponentInChildren<OscillatingMovement>();
    }
    
    protected override void OnEnable()
    {
        base.OnEnable();
       
        StatsSystem.Init();
       
        oscillatingMovement.Initialized();
        CanUseCard = false;
        
       
       
        RunSpeed = 10f;
        
        AttackRange = 2.5f;
       
       
        
        IsOriginalFacingRight = false;

        OnFinishedUsingCard = () => CanUseCard = false;
        OnDead = () => EnemyManager.Instance.RemoveEnemy(this);
        onPlayerTurnAction = param => IsPlanningState = true;
        onEnemyTurnAction = param => IsPlanningState = false;
        
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, onPlayerTurnAction);
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, onEnemyTurnAction);
        StateMachine.SetState(IdleState);
       
        
    }
            
    private void OnDisable()
    {
        ObserverManager<GameStateType>.Detach(GameStateType.PlayerTurn, onPlayerTurnAction);
        ObserverManager<GameStateType>.Detach(GameStateType.EnemyTurn, onEnemyTurnAction);
        
    }
    

    public async UniTask DoAction()
    {
        StartTurn(null);
      
        CanUseCard = true;
        await UniTask.WaitUntil(() => CanUseCard== false);
        EndTurn(null);
    }
  
    Func<bool> RunToTargetCondition() => () =>CanUseCard && CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.transform.position, transform.position) > AttackRange;
    
    Func<bool> InPlanningState() => () => IsPlanningState && !IsHurting;
    Func<bool> CanEnterUseCardState() => () =>!IsPlanningState && CanUseCard && CardStrategy != null && !MustReachTarget;
    
   
}
