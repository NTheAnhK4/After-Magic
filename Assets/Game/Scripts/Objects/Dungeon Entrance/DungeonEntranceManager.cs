
using System;
using System.Collections.Generic;
using System.Linq;


public class DungeonEntranceManager : Singleton<DungeonEntranceManager>
{
    public List<DungeonEntrance> DungeonEntrances = new List<DungeonEntrance>();
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (DungeonEntrances == null) DungeonEntrances = new List<DungeonEntrance>();
        if (DungeonEntrances.Count == 0) DungeonEntrances = transform.GetComponentsInChildren<DungeonEntrance>().ToList();
    }

    private void Start()
    {
        foreach (DungeonEntrance de in DungeonEntrances)
        {
            if(de == null) continue;
            de.LockDungeonEntrance();
        }
        DungeonEntrances[0].UnLockDungeonEntrance();
    }
}
