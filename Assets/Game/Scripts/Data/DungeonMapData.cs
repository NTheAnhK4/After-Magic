using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Dungeon Map Data", menuName = "Data/ Dungeon Map")]
public class DungeonMapData : ScriptableObject
{
    public RoomUIBtn RoomPrefab;
    public GameObject VerticalPath;
    public GameObject HorizontalPath;
    public List<DungeonRoomSpriteInfor> RoomSprites;
   
}

[Serializable]
public class DungeonRoomSpriteInfor
{
    
    public string Name;
    public Sprite Sprite;
}
