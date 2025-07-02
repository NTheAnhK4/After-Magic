

using System;
using UnityEngine;
using UnityEngine.UI;

using System.Reflection;
public class RoomUIBtn : ComponentBehaviour
{
    
    public RoomsManager RoomsManager;
    public DungeonRoomType DungeonRoomType;
    public RoomVisitState RoomVisitState;
    
    #region Sprite
    public Sprite RoomSpriteBeforeEnter;
    public Sprite RoomSpriteAfterEnter;
    #endregion
   
   

    #region Strategy

    public RoomEventStrategy StrategyBeforeEnter;
    public RoomEventStrategy StrategyAfterEnter;

    #endregion
   

    public Vector2Int RoomPosition;

    private Button roomBtn;
    private Image RoomImg;
    private GameObject roomFrame;

   
    private bool isBtnClicked;
   
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

    public void ResetRoom()
    {
        isBtnClicked = false;
        roomBtn.onClick.RemoveAllListeners();
        StrategyAfterEnter = null;
        StrategyBeforeEnter = null;
    }
    public void SetRoomVisitState(RoomVisitState newState)
    {
        RoomVisitState = newState;
        roomBtn.interactable = RoomVisitState != RoomVisitState.Inaccessible;
        
        if (RoomVisitState != RoomVisitState.Inaccessible)
        {
            roomBtn.onClick.RemoveAllListeners();
            roomBtn.onClick.AddListener(EnterRoom);
        }
       
    }

    public void SetVirtualRoom(bool isVirtualRoom)
    {
        roomBtn.interactable = RoomVisitState != RoomVisitState.Inaccessible;
        if (isVirtualRoom) return;

        if (RoomVisitState != RoomVisitState.Inaccessible)
        {
            roomBtn.onClick.RemoveAllListeners();
            roomBtn.onClick.AddListener(EnterRoom);
        }
        
    }

    public void SetRoomSprite() =>  RoomImg.sprite = RoomVisitState == RoomVisitState.Entered ? RoomSpriteAfterEnter : RoomSpriteBeforeEnter;


    private void OnEnable()
    {
        isBtnClicked = false;
    }

    private void OnDisable()
    {
        roomBtn.onClick.RemoveAllListeners();
    }
  

    private void EnterRoom()
    {
       
       SelectRoom();
       if (isBtnClicked) return;
       if (RoomVisitState != RoomVisitState.Entered && InGameManager.Instance != null) InGameManager.Instance.RoomsExplored++;
       isBtnClicked = true;
       RoomEventStrategy strategy = RoomVisitState == RoomVisitState.Entered ? StrategyAfterEnter : StrategyBeforeEnter;
       
       if (strategy == null)
       {
           Debug.LogWarning("Strategy is null");
           return;
       }


       RoomVisitState = RoomVisitState.Entered;
       strategy.OnEnter();
      
      
      
    }

    public void SetInteracableNeighboringRoom()
    {
        RoomsManager.SetRoomEntered(RoomPosition.x, RoomPosition.y);
        RoomsManager.SetRoomInteracable(RoomPosition.x - 1, RoomPosition.y);
        RoomsManager.SetRoomInteracable(RoomPosition.x + 1, RoomPosition.y);
        RoomsManager.SetRoomInteracable(RoomPosition.x, RoomPosition.y + 1);
        RoomsManager.SetRoomInteracable(RoomPosition.x, RoomPosition.y - 1);
    }

    public void SelectRoom()
    {
        if(InGameManager.Instance.CurrentRoom != null) InGameManager.Instance.CurrentRoom.DeselectRoom();
        InGameManager.Instance.DungeonRoomType = DungeonRoomType;

        InGameManager.Instance.CurrentRoom = this;

        roomFrame.SetActive(true);
    }

    public void DeselectRoom() => roomFrame.SetActive(false);
}
