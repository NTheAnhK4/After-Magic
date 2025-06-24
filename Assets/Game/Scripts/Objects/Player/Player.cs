using System;

using StateMachine;
using UnityEngine;



public class Player : Entity
{
    
    #region Unity Event Callback

    protected override void Awake()
    {
        base.Awake();
       
       
        OnFinishedUsingCard += () => InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
        OnDead += () => ObserverManager<GameEventType>.Notify(GameEventType.Lose);
        
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.transform.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    
        
       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        SetFacing(true);
        RunSpeed = 10f;
        
        AttackRange = 2.5f;
      
        IsOriginalFacingRight = true;
        StateMachine.SetState(IdleState);
        ObserverManager<GameStateType>.Attach(GameStateType.CollectingCard,EndTurn);
        ObserverManager<GameStateType>.Attach(GameStateType.DistributeCard, StartTurn);
    }

    private void OnDisable()
    {
        ObserverManager<GameStateType>.Detach(GameStateType.CollectingCard,EndTurn);
        ObserverManager<GameStateType>.Detach(GameStateType.DistributeCard, StartTurn);
    }

    #endregion

    

    Func<bool> RunToTargetCondition() => () => CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.transform.position, transform.position) > AttackRange;

   
    Func<bool> CanEnterUseCardState() => () => CardStrategy != null && !MustReachTarget;
}
