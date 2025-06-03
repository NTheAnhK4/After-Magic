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

    private void OnEnable()
    {
        RunSpeed = 10f;
       
        AttackRange = 2.5f;
        Damage = 1;
        MaxHP = 100;
        CurHP = 100;
        Armor = 0;
        
        IsOriginalFacingRight = true;
        StateMachine.SetState(IdleState);
    }

    #endregion
    
    Func<bool> RunToTargetCondition() => () => CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.transform.position, transform.position) > AttackRange;

   
    Func<bool> CanEnterUseCardState() => () => CardStrategy != null && !MustReachTarget;
}
