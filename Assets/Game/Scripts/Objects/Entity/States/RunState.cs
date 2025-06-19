using System;
using StateMachine;
using UnityEngine;

public class RunStateData : StateData
{
    public Vector2 TargetPosition { get; set; }
    public bool IsRunningToTarget { get; set; }
}
public class RunState : State<Entity>
{
    
    private RunStateData data;
  

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        if (stateData is RunStateData runStateData)
        {
            data = runStateData;
            entity.SetFacing(data.TargetPosition.x - entity.transform.position.x > 0);
            entity.IsRunningToTarget = data.IsRunningToTarget;
            if (data.IsRunningToTarget) entity.StandPoint = entity.transform.position;
        }
       
    }

    public override void Update()
    {
        base.Update();
        if (data == null) return;
        
        if (!data.IsRunningToTarget)
        {
          
            float distance = Vector2.Distance(entity.transform.position, data.TargetPosition);
            if (distance <= 0.02f)
            {
                entity.SetFacing(entity.IsOriginalFacingRight);
                entity.EnemyTarget = null;
                entity.MustReachTarget = false;
                entity.OnFinishedUsingCard?.Invoke();
                entity.StateMachine.ChangeState(entity.IdleState);
            }
        }
        else
        {
            if(Vector2.Distance(entity.transform.position,entity.EnemyTarget.transform.position) <= entity.AttackRange)
                entity.StateMachine.ChangeState(entity.UseCardState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        entity.transform.position = Vector2.MoveTowards(entity.transform.position, data.TargetPosition, entity.RunSpeed * Time.fixedDeltaTime);
    }

   

    public RunState(Entity entity, string animBoolName) : base(entity, animBoolName)
    {
    }
}