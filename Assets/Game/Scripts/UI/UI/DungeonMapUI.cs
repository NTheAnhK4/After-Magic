


using System;
using System.Collections.Generic;
using System.Persistence;
using AudioSystem;
using Game.UI;
using UnityEngine;

using UnityEngine.UI;
public enum RoomVisitState
{
    Inaccessible = 0,        
    ReachableNotEntered = 1,  
    Entered = 2               
}

[Serializable]
public class RoomData
{
    public int RoomType;
    public RoomVisitState AccessState;
}

[RequireComponent(typeof(RoomUIInteraction), typeof(RoomUISpawner))]
public class DungeonMapUI : UIView
{

    public DungeonFloorConfig DungeonFloorConfig;
    public DungeonMapData DungeonMapData;

    public RoomUISpawner RoomUISpawner;
    public RoomUIInteraction RoomUIInteraction;



    public RoomsManager RoomsManager;



    public List<List<RoomData>> rooms;
    public bool IsVirtualMap;
    public bool IsShown;
    public bool IsLoadData = false;

    [SerializeField] private Button exitBtn;

    protected override void Awake()
    {
        base.Awake();
        IsShown = false;
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (RoomsManager == null) RoomsManager = GetComponent<RoomsManager>();
        if (RoomUISpawner == null) RoomUISpawner = GetComponent<RoomUISpawner>();
        if (RoomUIInteraction == null) RoomUIInteraction = GetComponent<RoomUIInteraction>();

        if (exitBtn == null) exitBtn = transform.Find("Exit").GetComponent<Button>();



        RoomUISpawner.LoadComponent();
        RoomUIInteraction.LoadComponent();


    }

    public void LoadRoomData(List<RoomRowData> roomDatas)
    {
        rooms = new List<List<RoomData>>();
        for (int i = 0; i < roomDatas.Count; ++i)
        {
            var row = new List<RoomData>();
            for (int j = 0; j < roomDatas[i].Row.Count; ++j)
            {
                row.Add(roomDatas[i].Row[j]);
            }
            rooms.Add(row);
        }
    }

    private void GenerateRoomData()
    {


        if (DungeonFloorConfig == null)
        {
            Debug.LogWarning("DungeonFloorConfig is null");
            return;
        }

        DungeonFloor dungeonFloor = DungeonFloorConfig.GetRandomDungeonFloor();
        rooms = new List<List<RoomData>>();

        for (int i = 0; i < dungeonFloor.Height; ++i)
        {
            var row = new List<RoomData>();
            for (int j = 0; j < dungeonFloor.Width; ++j)
            {
                int roomId = dungeonFloor.RoomTypes[i].Row[j];
                var room = new RoomData
                {
                    RoomType = roomId,
                    AccessState = roomId != -1 ? RoomVisitState.Inaccessible : default
                };
                row.Add(room);
            }

            rooms.Add(row);
        }

        for (int i = 0; i < rooms.Count; ++i)
        {
            for (int j = 0; j < rooms[i].Count; ++j)
            {
                if (rooms[i][j].RoomType == 4)
                {
                    rooms[i][j].AccessState = RoomVisitState.ReachableNotEntered;

                    TrySetAccess(i + 1, j, dungeonFloor);
                    TrySetAccess(i - 1, j, dungeonFloor);
                    TrySetAccess(i, j + 1, dungeonFloor);
                    TrySetAccess(i, j - 1, dungeonFloor);
                }
            }
        }

        Bind();
        SaveLoadSystem.Instance.SaveGame();
    }

    private void Bind()
    {
        if (InGameManager.Instance != null)
        {
            
            InGameManager.Instance.dungeonSaveData.RoomDatas = new List<RoomRowData>();
            for (int i = 0; i < rooms.Count; ++i)
            {
                RoomRowData roomRowData = new RoomRowData();
                roomRowData.Row = new List<RoomData>();
                for (int j = 0; j < rooms[i].Count; ++j)
                {
                    roomRowData.Row.Add(rooms[i][j]);
                }

                InGameManager.Instance.dungeonSaveData.RoomDatas.Add(roomRowData);
            }

        }
    }

void TrySetAccess(int x, int y, DungeonFloor dungeonFloor)
    {
        if (x >= 0 && x < dungeonFloor.Height &&
            y >= 0 && y < dungeonFloor.Width &&
            dungeonFloor.RoomTypes[x].Row[y] != -1)
        {
           
            if (rooms[x][y] == null)
                rooms[x][y] = new RoomData();

            rooms[x][y].AccessState = RoomVisitState.ReachableNotEntered;
        }
    }

    public override void Show()
    {
        if(IsVirtualMap) ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        base.Show();
        exitBtn.gameObject.SetActive(IsVirtualMap);
        if(IsVirtualMap) exitBtn.onClick.AddListener(OnExitBtnClick );
        if (!IsShown)
        {
            if(!IsLoadData) GenerateRoomData();
            if (rooms == null)
            {
                Debug.Log("Room is null");
                return;
            }
            RoomUIInteraction.Init();
            RoomsManager. Build(rooms.Count, rooms[0].Count);
            RoomUISpawner.SpawnRooms(rooms);
            IsShown = true;
        }
        else RoomsManager.ShowRoom(IsVirtualMap);
       
        IsShown = true;
    }

    public override void Hide()
    {
        base.Hide();
        if(IsVirtualMap) ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
    }

    private async void OnExitBtnClick() => await UIScreen.HideUI<DungeonMapUI>();
 
    private void OnDisable()
    {
        if(IsVirtualMap) exitBtn.onClick.RemoveAllListeners();
    }


    
  
}

