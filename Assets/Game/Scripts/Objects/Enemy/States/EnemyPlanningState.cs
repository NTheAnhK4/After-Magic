
using StateMachine;
using UnityEngine;

public class EnemyPlanningStateData : StateData
{
    public Player Player { get; set; }
}

public class EnemyPlanningState : State<Enemy>
{
    private EnemyPlanningStateData data;
    
    public EnemyPlanningState(Enemy entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
      
        if (entity.IsNextActionPlanned) return;
        
        if (stateData is EnemyPlanningStateData planningData)
        {
            data = planningData;
            entity.EnemyTarget = data.Player;
            
        }
        
        EnemyCardData enemyCardData = GetCardStrategy();

        entity.ShowWarning(enemyCardData.WarningPrefab);
       
        entity.CardStrategy = enemyCardData.CardStrategy;
        entity.MustReachTarget = enemyCardData.CardStrategy.MustReachTarget;
    }

   
    private EnemyCardData GetCardStrategy()
    {
        int id = Random.Range(0, entity.CardStrategyAvailables.Count);
        return entity.CardStrategyAvailables[id];
    }
}
