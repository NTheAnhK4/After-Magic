using System.Collections;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

public class EnemyInteractState : State<Entity>
{
    public EnemyInteractState(Enemy entity, string animBoolName) : base(entity, animBoolName)
    {
    }
    
}
