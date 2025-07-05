
using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/World/WorldListData", fileName = "WorldsData")]
public class WorldListData : ScriptableObject
{
    public List<WorldData> WorldDatas;
}

[Serializable]
public class WorldData
{
    public string WorldName;
    public Sprite WorldSprite;
    public Sprite WorldBackgroundSprite;
    public string Description;
    public WorldEffect WorldEffect;
    public RoomEnemyGroupConfig RoomEnemyGroupConfig;
}
