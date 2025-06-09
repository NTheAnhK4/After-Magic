

using UnityEngine;

public class RoomsManager : RoomUIComponent
{
  
  
    private RoomUIBtn[][] dungeonFloorRooms;
    
   

    public void Build(int height, int width)
    {
        dungeonFloorRooms = new RoomUIBtn[height][];
        for (int i = 0; i < height; ++i) dungeonFloorRooms[i] = new RoomUIBtn[width];
      
    }

    public void AddRoom(int posX, int posY, RoomUIBtn roomUIBtn, DungeonRoomType dungeonRoomType)
    {
        roomUIBtn.RoomsManager = this;
        dungeonFloorRooms[posX][posY] = roomUIBtn;
        SetRoomUIInfor(roomUIBtn, dungeonRoomType);
    }

    public void FinishAddRoom()
    {
       
        for (int i = 0; i < dungeonFloorRooms.Length; ++i)
        {
            for (int j = 0; j < dungeonFloorRooms[i].Length; ++j)
            {
                if(dungeonFloorRooms[i][j] == null) continue;
                if (dungeonFloorRooms[i][j].DungeonRoomType == DungeonRoomType.Entry)
                {
                    dungeonFloorRooms[i][j].SelectRoom();
                    dungeonFloorRooms[i][j].SetRoomReachable(true);
                    dungeonFloorRooms[i][j].SetInteracableNeighboringRoom();
                    return;
                }
            }
        }
    }

    public void ResetRoom()
    {
        for (int i = 0; i < dungeonFloorRooms.Length; ++i)
        {
            for (int j = 0; j < dungeonFloorRooms[i].Length; ++j)
            {
                PoolingManager.Despawn(dungeonFloorRooms[i][j].gameObject);
            }
        }
        
    }

    public void ShowRoom(bool isVirtualMap)
    {
        for (int i = 0; i < dungeonFloorRooms.Length; ++i)
        {
            for (int j = 0; j < dungeonFloorRooms[i].Length; ++j)
            {
                if(dungeonFloorRooms[i][j] == null) continue;
                dungeonFloorRooms[i][j].SetVirtualRoom(isVirtualMap);
                dungeonFloorRooms[i][j].SetRoomSprite();
            }
        }
    }

    public void SetRoomInteracable(int i, int j)
    {
        if (i < 0 || i >= dungeonFloorRooms.Length) return;
        if (j < 0 || j >= dungeonFloorRooms[i].Length) return;

        if (dungeonFloorRooms[i][j] == null) return;
  
        dungeonFloorRooms[i][j].SetRoomReachable(true);
    }
    private void SetRoomUIInfor( RoomUIBtn roomUIBtn, DungeonRoomType dungeonRoomType)
    {

        roomUIBtn.SetRoomReachable(false);
        
        roomUIBtn.DungeonRoomType = dungeonRoomType;
        int beforeEnterID = 0;
        int afterEnterID = 0;
        switch (dungeonRoomType)
        {
            case DungeonRoomType.Battle:
                beforeEnterID = (int)dungeonRoomType;
                afterEnterID = (int)DungeonRoomType.Empty;
              
                roomUIBtn.StrategyBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[(int)dungeonRoomType].RoomEventStrategy;
                roomUIBtn.StrategyBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[(int)DungeonRoomType.Empty].RoomEventStrategy;
                break;
            case DungeonRoomType.Campfire:
            case DungeonRoomType.Door:
            case DungeonRoomType.Shop:
                beforeEnterID = (int)DungeonRoomType.Mystery;
                afterEnterID = (int)dungeonRoomType;
                break;
            case DungeonRoomType.Entry:
            case DungeonRoomType.Empty:
                beforeEnterID = (int)dungeonRoomType;
                afterEnterID = (int)dungeonRoomType;
                break;
            default:
                Debug.LogWarning(dungeonRoomType.ToString() + " is not defined in set room infor");
                break;

              
        }
        roomUIBtn.RoomSpriteBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[beforeEnterID].Sprite;
        roomUIBtn.RoomSpriteAfterEnter = DungeonMapUI.DungeonMapData.RoomInfor[afterEnterID].Sprite;

        roomUIBtn.StrategyBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[beforeEnterID].RoomEventStrategy;
        roomUIBtn.StrategyAfterEnter = DungeonMapUI.DungeonMapData.RoomInfor[afterEnterID].RoomEventStrategy;
        roomUIBtn.SetRoomSprite();
    }
}
