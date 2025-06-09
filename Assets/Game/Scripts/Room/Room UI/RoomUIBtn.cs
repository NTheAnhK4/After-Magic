
using UnityEngine;
using UnityEngine.UI;

public class RoomUIBtn : ComponentBehavior
{
    public RoomsManager RoomsManager;
    public DungeonRoomType DungeonRoomType;
    
    #region Sprite
    public Sprite RoomSpriteBeforeEnter;
    public Sprite RoomSpriteAfterEnter;
    #endregion
   
    public bool isEntered = false;

    #region Strategy

    public RoomEventStrategy StrategyBeforeEnter;
    public RoomEventStrategy StrategyAfterEnter;

    #endregion
   

    public Vector2Int RoomPosition;

    private Button roomBtn;
    private Image RoomImg;
    private GameObject roomFrame;

    private bool _reachable;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (roomBtn == null) roomBtn = transform.Find("RoomBtn").GetComponent<Button>();
        if (RoomImg == null) RoomImg = transform.Find("RoomBtn").GetComponent<Image>();
        if (roomFrame == null)
        {
            roomFrame = transform.Find("Frame").gameObject;
            roomFrame.gameObject.SetActive(false);
        }
    }

    public void SetRoomReachable(bool reachable)
    {
        roomBtn.interactable = reachable;
        _reachable = reachable;
        if(reachable) roomBtn.onClick.AddListener(EnterRoom);
    }

    public void SetVirtualRoom(bool isVirtualRoom)
    {
        if (isVirtualRoom) return;
        roomBtn.interactable = _reachable;
        if(_reachable) roomBtn.onClick.AddListener(EnterRoom);
    }

    public void SetRoomSprite() =>  RoomImg.sprite = isEntered ? RoomSpriteAfterEnter : RoomSpriteBeforeEnter;


   

    private void OnDisable()
    {
        roomBtn.onClick.RemoveAllListeners();
    }

    private void EnterRoom()
    {
        
       SelectRoom();
       RoomEventStrategy strategy = isEntered ? StrategyAfterEnter : StrategyBeforeEnter;
       if (strategy == null)
       {
           Debug.LogWarning("Strategy is null");
           return;
       }
       strategy.OnEnter();
       isEntered = true;
    }

    public void SetInteracableNeighboringRoom()
    {
     
        RoomsManager.SetRoomInteracable(RoomPosition.x - 1, RoomPosition.y);
        RoomsManager.SetRoomInteracable(RoomPosition.x + 1, RoomPosition.y);
        RoomsManager.SetRoomInteracable(RoomPosition.x, RoomPosition.y + 1);
        RoomsManager.SetRoomInteracable(RoomPosition.x, RoomPosition.y - 1);
    }

    public void SelectRoom()
    {
        if(InGameManager.Instance.CurrentRoom != null) InGameManager.Instance.CurrentRoom.DeselectRoom();
        InGameManager.Instance.DungeonRoomType = this.DungeonRoomType;

        InGameManager.Instance.CurrentRoom = this;

        roomFrame.SetActive(true);
    }

    public void DeselectRoom() => roomFrame.SetActive(false);
}
