
using System.Collections.Generic;
using UnityEngine;


public class RoomUISpawner : RoomUIComponent
{
   
    [SerializeField] private Transform RoomHolder;

    
    public float roomDistance = 100f;

    [SerializeField] private List<GameObject> pathList = new List<GameObject>();
    [SerializeField] private List<GameObject> roomList = new List<GameObject>();
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (RoomHolder == null) RoomHolder = transform.Find("Rooms");
    }

    public void SpawnRooms(List<List<RoomData>> rooms)
    {
        
        for (int i = 0; i < rooms.Count; ++i)
        {
            for (int j = 0; j < rooms[i].Count; ++j)
            {
                SpawnRoom(i,j,rooms);
            }
        }
        DungeonMapUI.RoomsManager.FinishAddRoom();
    }
   

  
 

    private Vector3 GetPosition(int posX, int posY, List<List<RoomData>> rooms) => new Vector3( posX - rooms.Count / 2,rooms[0].Count / 2 - posY , 0) * (2 * roomDistance);
    private void SpawnRoom(int posY, int posX, List<List<RoomData>> rooms)
    {
        if (rooms[posY][posX].RoomType == -1) return;

        Vector3 roomPosition = GetPosition(posX, posY, rooms);
      
        GameObject roomGO = PoolingManager.Spawn(DungeonMapUI.DungeonMapData.RoomPrefab.gameObject, roomPosition, default, RoomHolder);
 
        roomList.Add(roomGO);
        
        RectTransform rect = roomGO.GetComponent<RectTransform>();
        rect.anchoredPosition = roomPosition;
        roomGO.transform.localScale = Vector3.one;
        
        RoomUIBtn roomUIBtn = roomGO.GetComponent<RoomUIBtn>();
        
        DungeonMapUI.RoomsManager.AddRoom(posY, posX, roomUIBtn, (DungeonRoomType)rooms[posY][posX].RoomType, rooms[posY][posX].AccessState);
        
        roomUIBtn.RoomPosition = new Vector2Int(posY, posX);
        
       
       
        SpawnPath(posY,posX, rooms);
    }

   
    private void SpawnPath(int posY, int posX, List<List<RoomData>> rooms)
    {
        if (posY > 0 && rooms[posY - 1][posX].RoomType != -1)
        {
            Vector3 spawnPos = GetPosition(posX, posY, rooms) + new Vector3(0, roomDistance, 0);
            GameObject prefab = DungeonMapUI.DungeonMapData.VerticalPath;
            SpawnPath(posY, posX, prefab, spawnPos);
        }

        if (posX > 0 && rooms[posY][posX - 1].RoomType != -1)
        {
            Vector3 spawnPos = GetPosition(posX, posY,rooms) - new Vector3(roomDistance, 0, 0);
            GameObject prefab = DungeonMapUI.DungeonMapData.HorizontalPath;
            SpawnPath(posY, posX, prefab, spawnPos);
        }
    }

    private void SpawnPath(int posY, int posX, GameObject prefab, Vector3 spawnPos)
    {
        GameObject pathGO = PoolingManager.Spawn(prefab, spawnPos, prefab.transform.rotation, RoomHolder);
        pathList.Add(pathGO);
        pathGO.transform.localScale = Vector3.one;

        RectTransform rect = pathGO.GetComponent<RectTransform>();
        rect.anchoredPosition = spawnPos;
    }

    public void ResetRoom()
    {
       
        foreach (GameObject path in pathList)
        {
            if(path != null) PoolingManager.Despawn(path);
        }
        pathList.Clear();

        foreach (GameObject room in roomList)
        {
            if(room != null) PoolingManager.Despawn(room);
        }
        roomList.Clear();
    }
}
