

using Game.UI;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(RoomUIInteraction), typeof(RoomUISpawner))]
public class DungeonMapUI : UIView
{
    public DungeonFloorConfig DungeonFloorConfig;
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
        
        
        
        RoomUISpawner.LoadComponent();
        RoomUIInteraction.LoadComponent();
        
       
    }

    private void SetRoomData()
    {
      

        if (DungeonFloorConfig == null)
        {
            Debug.LogWarning("DungeonFloorConfig is null");
            return;
        }
        
        DungeonFloor dungeonFloor = DungeonFloorConfig.GetRandomDungeonFloor();
        rooms = new int[dungeonFloor.Height][];
        for (int i = 0; i < dungeonFloor.Height; ++i)
        {
            rooms[i] = new int[dungeonFloor.Width];
            for (int j = 0; j < dungeonFloor.Width; ++j)
            {
                rooms[i][j] = dungeonFloor.RoomTypes[i].Row[j];
            }
        }
    }

    public override void Show()
    {
        
        base.Show();
        exitBtn.gameObject.SetActive(IsVirtualMap);
        if(IsVirtualMap) exitBtn.onClick.AddListener(OnExitBtnClick );
        if (!IsShown)
        {
            SetRoomData();
            if (rooms == null)
            {
                Debug.Log("Room is null");
                return;
            }
            RoomUIInteraction.Init();
            RoomsManager. Build(rooms.Length, rooms[0].Length);
            RoomUISpawner.SpawnRooms(rooms);
            IsShown = true;
        }
        else RoomsManager.ShowRoom(IsVirtualMap);
       
        IsShown = true;
    }

    private async void OnExitBtnClick() => await UIScreen.HideUI<DungeonMapUI>();
 
    private void OnDisable()
    {
        if(IsVirtualMap) exitBtn.onClick.RemoveAllListeners();
    }

    
}
