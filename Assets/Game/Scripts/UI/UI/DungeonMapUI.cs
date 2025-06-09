
using System;
using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(RoomUIInteraction), typeof(RoomUISpawner))]
public class DungeonMapUI : UIView
{
  
    public DungeonMapData DungeonMapData;
    
    public RoomUISpawner RoomUISpawner;
    public RoomUIInteraction RoomUIInteraction;
  
    

    public RoomsManager RoomsManager;

  

    public int[][] rooms;
    public bool IsVirtualMap;
    public bool IsShown;
    
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
        
        
        rooms = new int[5][];
        for (int i = 0; i < 5; ++i)
        {
            rooms[i] = new int[5];
            for (int j = 0; j < 5; ++j)
            {
                if (i == 0) rooms[i][j] = j == 4 ? 0 : -1;
                if (i == 1) rooms[i][j] = (j == 1 || j == 2 || j == 4) ? 0 : -1;
                if (i == 2)
                {
                    if (j == 1 || j == 4) rooms[i][j] = 0;
                    else if (j == 0 || j == 3) rooms[i][j] = -1;
                    else rooms[i][j] = 4;                    
                }

                if (i == 3) rooms[i][j] = (j == 1 || j == 4) ? 0 : -1;
                if (i == 4)
                {
                    if (j == 0) rooms[i][j] = 2;
                    else if (j == 1) rooms[i][j] = 6;
                    else if (j == 2 || j == 4) rooms[i][j] = 0;
                    else rooms[i][j] = 1;
                }
            }
        }
        RoomUISpawner.LoadComponent();
        RoomUIInteraction.LoadComponent();
        RoomUIInteraction.Init();
       
    }
    

    public override void Show()
    {
        
        base.Show();
        exitBtn.gameObject.SetActive(IsVirtualMap);
        if(IsVirtualMap) exitBtn.onClick.AddListener(() => UIScreen.HideUI<DungeonMapUI>());
        if (!IsShown)
        {
            if (rooms == null)
            {
                Debug.Log("Room is null");
                return;
            }
            RoomsManager. Build(rooms.Length, rooms[0].Length);
            RoomUISpawner.SpawnRooms(rooms);
            IsShown = true;
        }
        else RoomsManager.ShowRoom(IsVirtualMap);
       
        IsShown = true;
    }

 
    private void OnDisable()
    {
        if(IsVirtualMap) exitBtn.onClick.RemoveAllListeners();
    }

    
}
