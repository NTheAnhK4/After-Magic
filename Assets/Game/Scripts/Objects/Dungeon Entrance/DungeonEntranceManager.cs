
using System;
using System.Collections.Generic;
using System.Linq;
using SaveGame;


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
       LockAllDungeonEntrance();
       if(SaveLoadSystem.Instance.GameData == null) DungeonEntrances[0].UnLockDungeonEntrance();
       else
       {
           for(int i = 0;  i <= SaveLoadSystem.Instance.GameData.WorldUnlockedID; ++i) DungeonEntrances[i].UnLockDungeonEntrance();
       }
    }

    public void LockAllDungeonEntrance()
    {
        foreach (DungeonEntrance de in DungeonEntrances)
        {
            if(de == null) continue;
            de.LockDungeonEntrance();
        }
    }
}
