
using UnityEngine;

public abstract class CardStrategy : ScriptableObject
{
    public string AnimName;
    public bool MustReachTarget;

    public abstract void Apply(Transform owner, Transform enemy);
    public abstract void Execute();

    public abstract void Remove();
    public virtual bool HasFinisedUsingCard() => true;
}
