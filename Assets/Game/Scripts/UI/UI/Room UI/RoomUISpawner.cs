
using UnityEngine;
using UnityEngine.UI;

public class RoomUISpawner : RoomUIComponent
{
    public DungeonMapData DungeonMapData;
    [SerializeField] private Transform RoomHolder;

    
    public float roomDistance = 100f;
    

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (RoomHolder == null) RoomHolder = transform.Find("Rooms");
    }

    public void SpawnRooms(int[][] rooms)
    {
      
        for (int i = 0; i < rooms.Length; ++i)
        {
            for (int j = 0; j < rooms[i].Length; ++j)
            {
                SpawnRoom(i,j,rooms);
            }
        }
    }
   

  
    public void ShowRooms()
    {
        foreach (Transform room in RoomHolder)
        {
            RoomUIBtn roomUIBtn = room.GetComponent<RoomUIBtn>();
            if(roomUIBtn == null) continue;
            roomUIBtn.Interacable = !dungeonMapUI.IsVirtualMap;
            roomUIBtn.Init();
        }
    }

    private Vector3 GetPosition(int posX, int posY, int[][] rooms) => new Vector3( posX - rooms.Length / 2,rooms[0].Length / 2 - posY , 0) * (2 * roomDistance);
    private void SpawnRoom(int posY, int posX, int[][] rooms)
    {
        if (rooms[posY][posX] == -1) return;

        Vector3 roomPosition = GetPosition(posX, posY, rooms);
      
        GameObject roomGO = PoolingManager.Spawn(DungeonMapData.RoomPrefab.gameObject, roomPosition, default, RoomHolder);
 
        RectTransform rect = roomGO.GetComponent<RectTransform>();
        rect.anchoredPosition = roomPosition;
        roomGO.transform.localScale = Vector3.one;
        
        RoomUIBtn roomUIBtn = roomGO.GetComponent<RoomUIBtn>();
        SetRoomUIInfor((DungeonRoomType)rooms[posY][posX], roomUIBtn);
       
        SpawnPath(posY,posX, rooms);
    }

    private void SetRoomUIInfor(DungeonRoomType dungeonRoomType, RoomUIBtn roomUIBtn)
    {
        roomUIBtn.Interacable = !dungeonMapUI.IsVirtualMap;
      
        roomUIBtn.OnButtonClick = () => dungeonMapUI.UIScreen.HideUI();
        roomUIBtn.DungeonRoomType = dungeonRoomType;
        switch (dungeonRoomType)
        {
            case DungeonRoomType.Battle:
                roomUIBtn.RoomSpriteBeforeEnter = DungeonMapData.RoomSprites[(int)dungeonRoomType].Sprite;
                roomUIBtn.RoomSpriteAfterEnter = DungeonMapData.RoomSprites[(int)DungeonRoomType.Empty].Sprite;
                break;
            case DungeonRoomType.Campfire:
            case DungeonRoomType.Door:
            case DungeonRoomType.Shop:
                roomUIBtn.RoomSpriteBeforeEnter = DungeonMapData.RoomSprites[(int)DungeonRoomType.Mystery].Sprite;
                roomUIBtn.RoomSpriteAfterEnter = DungeonMapData.RoomSprites[(int)dungeonRoomType].Sprite;
                break;
            case DungeonRoomType.Entry:
            case DungeonRoomType.Empty:
                roomUIBtn.RoomSpriteBeforeEnter = DungeonMapData.RoomSprites[(int)dungeonRoomType].Sprite;
                roomUIBtn.RoomSpriteAfterEnter = DungeonMapData.RoomSprites[(int)dungeonRoomType].Sprite;
                break;
            default:
                Debug.LogWarning(dungeonRoomType.ToString() + " is not defined in set room infor");
                break;
        }
        roomUIBtn.Init();
    }
    private void SpawnPath(int posY, int posX, int[][] rooms)
    {
        if (posY > 0 && rooms[posY - 1][posX] != -1)
        {
            Vector3 spawnPos = GetPosition(posX, posY, rooms) + new Vector3(0, roomDistance, 0);
            GameObject prefab = DungeonMapData.VerticalPath;
            SpawnPath(posY, posX, prefab, spawnPos);
        }

        if (posX > 0 && rooms[posY][posX - 1] != -1)
        {
            Vector3 spawnPos = GetPosition(posX, posY,rooms) - new Vector3(roomDistance, 0, 0);
            GameObject prefab = DungeonMapData.HorizontalPath;
            SpawnPath(posY, posX, prefab, spawnPos);
        }
    }

    private void SpawnPath(int posY, int posX, GameObject prefab, Vector3 spawnPos)
    {
        GameObject pathGO = PoolingManager.Spawn(prefab, spawnPos, prefab.transform.rotation, RoomHolder);
        pathGO.transform.localScale = Vector3.one;

        RectTransform rect = pathGO.GetComponent<RectTransform>();
        rect.anchoredPosition = spawnPos;
    }
}
