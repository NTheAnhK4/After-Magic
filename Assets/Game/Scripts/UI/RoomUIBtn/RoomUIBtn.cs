
using System;
using UnityEngine;
using UnityEngine.UI;

public class RoomUIBtn : ComponentBehavior
{
    public DungeonRoomType DungeonRoomType;
    public Sprite RoomSpriteBeforeEnter;
    public Sprite RoomSpriteAfterEnter;
    public bool isEntered = false;
    public bool Interacable;
    public Action OnButtonClick;

    private Button roomBtn;
    private Image image;
    
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (roomBtn == null) roomBtn = GetComponent<Button>();
        if (image == null) image = GetComponent<Image>();
    }

   

    public void Init()
    {
        image.sprite = isEntered ? RoomSpriteAfterEnter : RoomSpriteBeforeEnter; 
        if (Interacable) roomBtn.onClick.AddListener(EnterRoom);
        roomBtn.interactable = Interacable;
    }

    private void OnDisable()
    {
        if(Interacable) roomBtn.onClick.RemoveAllListeners();
    }

    private void EnterRoom()
    {
        isEntered = true;
        InGameManager.Instance.DungeonRoomType = this.DungeonRoomType;
        OnButtonClick?.Invoke();
    }
}
