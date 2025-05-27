
using StateMachine;

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
        if (stateData is EnemyPlanningStateData planningData)
        {
            data = planningData;
            entity.EnemyTarget = data.Player.transform;
            
        }
        entity.PredictedActionTrf.gameObject.SetActive(true);
        entity.CardStrategy = entity.Test;
        entity.MustReachTarget = entity.Test.MustReachTarget;

    }

    public override void OnExit()
    {
        base.OnExit();
        entity.PredictedActionTrf.gameObject.SetActive(false);
    }
}
