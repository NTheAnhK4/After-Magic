using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "Dungeon Map Data", menuName = "Data/Dungeon/Dungeon Map")]
public class DungeonMapData : ScriptableObject
{
    public GameObject RoomPrefab;
    public GameObject VerticalPath;
    public GameObject HorizontalPath;
    public List<DungeonRoomSpriteInfor> RoomInfor;
   
}

[Serializable]
public class DungeonRoomSpriteInfor
{
    
    public string Name;
    public Sprite Sprite;
    public RoomEventStrategy RoomEventStrategy;
}
