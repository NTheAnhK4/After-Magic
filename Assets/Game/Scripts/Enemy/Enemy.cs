
using System;
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;

public class Enemy : Entity, IPlannerEntity
{
    public CardStrategy Test;
    #region State
    
    public EnemyPlanningState PlanningState { get; set; }

    #endregion

    #region Planning

    public bool IsPlanningState;
    public bool CanUseCard;
    public EnemyActionType PredictedAction { get; set; }
    public Transform PredictedActionTrf { get; set; }
    private Player player;
    #endregion
   
    protected override void Awake()
    {
        base.Awake();
        CanUseCard = false;
        
       
       
        RunSpeed = 5f;
        IsRunningToTarget = false;
        AttackRange = 2.5f;
        IsOriginalFacingRight = false;

        OnFinishedUsingCard += () => CanUseCard = false;
        PlanningState = new EnemyPlanningState(this, "Idle");
        

        player = FindObjectOfType<Player>();
        
       
        Any(PlanningState, new FuncPredicate(InPlanningState()), () => new EnemyPlanningStateData(){Player = player});
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    

       
        StateMachine.SetState(IdleState);
    }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (PredictedActionTrf == null) PredictedActionTrf = transform.Find("UI").Find("NextAction");
    }

    private void OnEnable()
    {
        if(EnemyManager.Instance != null) EnemyManager.Instance.AddEnemy(this);
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, param => IsPlanningState = true);
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, param => IsPlanningState = false);
    }
            
    private void OnDisable()
    {
        EnemyManager.Instance.RemoveEnemy(this);
        ObserverManager<GameStateType>.DetachAll();
    }

    public async UniTask DoAction()
    {
        CanUseCard = true;
      
        await UniTask.WaitUntil(() => CanUseCard== false);
        
    }
  
    Func<bool> RunToTargetCondition() => () =>CanUseCard && CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.position, transform.position) > AttackRange;

    
    Func<bool> InPlanningState() => () => IsPlanningState;
    Func<bool> CanEnterUseCardState() => () =>!IsPlanningState && CanUseCard && CardStrategy != null && !MustReachTarget;

}
