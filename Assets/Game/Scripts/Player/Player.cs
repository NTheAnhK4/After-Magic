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
       
        AttackRange = 2.5f;
        Damage = 1;
        MaxHP = 100;
        CurHP = 100;
        Armor = 0;
        
        IsOriginalFacingRight = true;
        OnFinishedUsingCard += () => GameManager.Instance.SetTurn(GameStateType.PlayerTurn);
        OnDead += () => Debug.Log("Lose");
        
        Any(RunState, new FuncPredicate(RunToTargetCondition()), () => new RunStateData{TargetPosition = EnemyTarget.transform.position, IsRunningToTarget = true});
        At(IdleState, UseCardState, new FuncPredicate(CanEnterUseCardState()));    
       
        
            
            
        StateMachine.SetState(IdleState);
    }

    private void Start()
    {
        ObserverManager<CardTargetType>.Attach(CardTargetType.Player, param => ToggleAim((bool)param));
    }

    #endregion
    
    Func<bool> RunToTargetCondition() => () => CardStrategy != null  && MustReachTarget && EnemyTarget != null
                                               && Vector2.Distance(EnemyTarget.transform.position, transform.position) > AttackRange;

   
    Func<bool> CanEnterUseCardState() => () => CardStrategy != null && !MustReachTarget;
}
