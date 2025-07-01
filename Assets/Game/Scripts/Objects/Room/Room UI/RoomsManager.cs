

using UnityEngine;



public class RoomsManager : RoomUIComponent
{
  
  
    private RoomUIBtn[][] dungeonFloorRooms;
    
   

    public void Build(int height, int width)
    {
        dungeonFloorRooms = new RoomUIBtn[height][];
        for (int i = 0; i < height; ++i) dungeonFloorRooms[i] = new RoomUIBtn[width];
      
    }

    public void AddRoom(int posX, int posY, RoomUIBtn roomUIBtn, DungeonRoomType dungeonRoomType, RoomVisitState roomVisitState)
    {
        roomUIBtn.RoomsManager = this;
        roomUIBtn.ResetRoom();
        dungeonFloorRooms[posX][posY] = roomUIBtn;
        SetRoomUIInfor(roomUIBtn, dungeonRoomType);
        if(dungeonRoomType == DungeonRoomType.Entry) roomUIBtn.SelectRoom();
        roomUIBtn.SetRoomVisitState(roomVisitState);
        roomUIBtn.SetRoomSprite();
        
    }

    public void FinishAddRoom()
    {
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
        if (dungeonFloorRooms[i][j].RoomVisitState == RoomVisitState.Inaccessible)
        {
            dungeonFloorRooms[i][j].SetRoomVisitState(RoomVisitState.ReachableNotEntered);
            if (DungeonMapUI != null) DungeonMapUI.rooms[i][j].AccessState = RoomVisitState.ReachableNotEntered;

        }
        
    }

    public void SetRoomEntered(int i, int j)
    {
        if(dungeonFloorRooms[i][j] == null || DungeonMapUI == null) return;
        DungeonMapUI.rooms[i][j].AccessState = RoomVisitState.Entered;
        
    }
    private void SetRoomUIInfor( RoomUIBtn roomUIBtn, DungeonRoomType dungeonRoomType)
    {

        
        
        roomUIBtn.DungeonRoomType = dungeonRoomType;
        int beforeEnterSpriteID = 0;
        int afterEnterSpriteID = 0;
        
        int beforeEnterStrategyID = 0;
        int afterEnterStrategyID = 0;
        switch (dungeonRoomType)
        {
            case DungeonRoomType.Battle:
                beforeEnterSpriteID = (int)dungeonRoomType;
                afterEnterSpriteID = (int)DungeonRoomType.Empty;

                beforeEnterStrategyID = beforeEnterSpriteID;
                afterEnterStrategyID = afterEnterSpriteID;
                break;
            case DungeonRoomType.Campfire:
                beforeEnterSpriteID = (int)DungeonRoomType.Mystery;
                afterEnterSpriteID = (int)dungeonRoomType;

                beforeEnterStrategyID = afterEnterSpriteID;
                afterEnterStrategyID = afterEnterSpriteID;
                break;
            case DungeonRoomType.Door:
                beforeEnterSpriteID = (int)DungeonRoomType.Mystery;
                afterEnterSpriteID = (int)dungeonRoomType;

                beforeEnterStrategyID = (int)DungeonRoomType.Battle;
                afterEnterStrategyID = (int)dungeonRoomType;
                break;
            case DungeonRoomType.Shop:
                beforeEnterSpriteID = (int)DungeonRoomType.Mystery;
                afterEnterSpriteID = (int)dungeonRoomType;
                
                beforeEnterStrategyID = (int)dungeonRoomType;
                afterEnterStrategyID = (int)dungeonRoomType;
                break;
            case DungeonRoomType.Entry:
            case DungeonRoomType.Empty:
               
                beforeEnterSpriteID = (int)dungeonRoomType;
                afterEnterSpriteID = (int)dungeonRoomType;

                beforeEnterStrategyID = beforeEnterSpriteID;
                afterEnterStrategyID = afterEnterSpriteID;
                break;
            default:
                Debug.LogWarning(dungeonRoomType.ToString() + " is not defined in set room infor");
                break;

              
        }

       
        roomUIBtn.RoomSpriteBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[beforeEnterSpriteID].Sprite;
        roomUIBtn.RoomSpriteAfterEnter = DungeonMapUI.DungeonMapData.RoomInfor[afterEnterSpriteID].Sprite;

        roomUIBtn.StrategyBeforeEnter = DungeonMapUI.DungeonMapData.RoomInfor[beforeEnterStrategyID].RoomEventStrategy;
        roomUIBtn.StrategyAfterEnter = DungeonMapUI.DungeonMapData.RoomInfor[afterEnterStrategyID].RoomEventStrategy;
        roomUIBtn.SetRoomSprite();
    }
}
