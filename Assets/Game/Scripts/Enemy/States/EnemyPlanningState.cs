
using StateMachine;
using UnityEngine;

public class EnemyPlanningStateData : StateData
{
    public Player Player { get; set; }
}

public class EnemyPlanningState : State<Enemy>
{
    private EnemyPlanningStateData data;
    private GameObject warningPrefab;
    public EnemyPlanningState(Enemy entity, string animBoolName) : base(entity, animBoolName)
    {
    }

    public override void OnEnter(StateData stateData = null)
    {
        base.OnEnter(stateData);
        if (stateData is EnemyPlanningStateData planningData)
        {
            data = planningData;
            entity.EnemyTarget = data.Player;
            
        }
        
        CardStrategy cardStrategy = GetCardStrategy();

        warningPrefab = PoolingManager.Spawn(cardStrategy.WarningPrefab, entity.PredictedActionTrf);
        
        entity.CardStrategy = cardStrategy;
        entity.MustReachTarget = cardStrategy.MustReachTarget;
    }

    public override void OnExit()
    {
        base.OnExit();
        if (warningPrefab != null) PoolingManager.Despawn(warningPrefab);
       
    }


    private CardStrategy GetCardStrategy()
    {
        int id = Random.Range(0, entity.CardStrategyAvailables.Count);
        return entity.CardStrategyAvailables[id];
    }
}
