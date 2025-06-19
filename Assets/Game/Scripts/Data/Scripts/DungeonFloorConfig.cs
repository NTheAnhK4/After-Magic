using System;

using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Data/Dungeon/DungeonFloorConfig", fileName = "DungeonFloorConfig")]
public class DungeonFloorConfig : ScriptableObject
{
    public List<DungeonFloor> DungeonFloors;

    public DungeonFloor GetRandomDungeonFloor()
    {
        if (DungeonFloors == null || DungeonFloors.Count == 0) return null;
        return DungeonFloors[Random.Range(0, DungeonFloors.Count)];
    }
}

[Serializable]
public class IntList
{
    public List<int> Row = new List<int>();
}

[Serializable]
public class DungeonFloor
{
    public int Width;
    public int Height;
    public List<IntList> RoomTypes = new List<IntList>();
    public void ResizeMatrix()
    {
        
        while (RoomTypes.Count < Height)
            RoomTypes.Add(new IntList());
        while (RoomTypes.Count > Height)
            RoomTypes.RemoveAt(RoomTypes.Count - 1);

        
        foreach (var row in RoomTypes)
        {
            while (row.Row.Count < Width)
                row.Row.Add(0);
            while (row.Row.Count > Width)
                row.Row.RemoveAt(row.Row.Count - 1);
        }
    }
}
