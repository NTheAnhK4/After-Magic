using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[CreateAssetMenu(menuName = "Data/Dungeon/RoomEnemyGroupConfig", fileName = "RoomEnemyGroupConfig")]
public class RoomEnemyGroupConfig : ScriptableObject
{
    public List<EnemyGroup> NoviceGroups;
    public List<EnemyGroup> VeteranGroups;
    public List<EnemyGroup> EliteGroups;
    public List<EnemyGroup> NightMareGroups;
    public List<EnemyGroup> BossGroups;

    public List<EnemyGroup> GetGroupListByFloor(int floor, int deepest, bool isBossRoom)
    {
        if (isBossRoom) return BossGroups;
        float ratio = (float)floor / deepest;
        if (ratio <= 0.25f) return NoviceGroups;
        if (ratio <= 0.5f) return VeteranGroups;
        if (ratio <= 0.75f) return EliteGroups;
        return NightMareGroups;
    }

    public EnemyGroup GetRandomGroupByFloor(int floor, int deepest, bool isBossRoom)
    {
        List<EnemyGroup> list = GetGroupListByFloor(floor, deepest, isBossRoom);
        if (list == null || list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }
}

[Serializable]
public class EnemyGroup
{
    public string GroupName;
    public List<Enemy> Enemies;
}