
using StateMachine;
using UnityEngine;

public class PlayerRunStataData : StateData
{
    public Vector2 TargetPosition { get; set; }
    public bool IsRunningToEnemy { get; set; }
}
public class PlayerRunState : PlayerState
{
   
    private PlayerRunStataData data;
    public PlayerRunState(Player entity, string animName) : base(entity, animName)
    {
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        if (stateData is PlayerRunStataData runStataData)
        {
            data = runStataData;
            entity.SetFacing(data.TargetPosition.x - entity.transform.position.x > 0);
            entity.IsRunningToEnemy = data.IsRunningToEnemy;
        }
    }


    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (data == null) return;
        if (!data.IsRunningToEnemy)
        {
            float distance = Vector2.Distance(entity.transform.position, data.TargetPosition);
            if (distance <= 0.02f)
            {
                entity.SetFacing(true);
                entity.EnemyTarget = null;
            }
        }
        entity.transform.position = Vector2.MoveTowards(entity.transform.position, data.TargetPosition, entity.Data.RunSpeed * Time.fixedDeltaTime);
    }

  
}
