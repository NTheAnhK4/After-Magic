using System;

using StateMachine;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : Entity
{
   
    

    #region Unity Event Callback

    protected override void Awake()
    {
        base.Awake();
       
        RunSpeed = 5f;
        IsRunningToTarget = false;
        AttackRange = 2.5f;
        
        IsOriginalFacingRight = true;
        OnFinishedUsingCard += () => GameManager.Instance.SetTurn(GameStateType.PlayerTurn);
        
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    
       
        
            
            
        StateMachine.SetState(IdleState);
    }
    
    #endregion
    
    Func<bool> RunToTargetCondition() => () => CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.position, transform.position) > AttackRange;

   
    Func<bool> CanEnterUseCardState() => () => CardStrategy != null && !MustReachTarget;
}
