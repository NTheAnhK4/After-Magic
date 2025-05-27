using StateMachine;
using UnityEngine;

public interface IPlannerEntity : IEntity
{
    public EnemyActionType PredictedAction { get; set; }
    public Transform PredictedActionTrf { get; set; }
}