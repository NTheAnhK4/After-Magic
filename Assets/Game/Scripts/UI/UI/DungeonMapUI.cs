
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
  
    
    public RoomUISpawner RoomUISpawner;
    public RoomUIInteraction RoomUIInteraction;
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
      
        if (RoomUISpawner == null) RoomUISpawner = GetComponent<RoomUISpawner>();
        if (RoomUIInteraction == null) RoomUIInteraction = GetComponent<RoomUIInteraction>();
        if (exitBtn == null) exitBtn = transform.Find("Exit").GetComponent<Button>();
        rooms = new int[5][];
        for (int i = 0; i < 5; ++i)
        {
            rooms[i] = new int[5];
            for (int j = 0; j < 5; ++j)
            {
                if (i == 0) rooms[i][j] = -1;
                if ( i == 3) rooms[i][j] = (j == 4) ? 0 : -1;
                if (i == 1 || i == 2) rooms[i][j] = (j == 1 || j == 2 || j == 4) ? 0 : -1;
                if (i == 4) rooms[i][j] = 0; 
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
        if(IsVirtualMap) exitBtn.onClick.AddListener(() => UIScreen.HideUI());
        if(!IsShown) RoomUISpawner.SpawnRooms(rooms);
        else RoomUISpawner.ShowRooms();
       
        IsShown = true;
    }

 
    private void OnDisable()
    {
        if(IsVirtualMap) exitBtn.onClick.RemoveAllListeners();
    }

    
}
