using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;

public class InGameUICtrl : ComponentBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CardManager cardManager;
 
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (canvas == null) canvas = GetComponent<Canvas>();
        if (cardManager == null) cardManager = transform.Find("Screen/Card Desk/Card In Hand").GetComponent<CardManager>();
    }

    public void InitData()
    {
        canvas.worldCamera = Camera.main;
        cardManager.InitData();

        DungeonSaveData dungeonSaveData = InGameManager.Instance.dungeonSaveData;
        if (dungeonSaveData.RoomDatas == null || dungeonSaveData.RoomDatas.Count == 0) return;
        DungeonMapUI dungeonMapUI = UIScreen.Instance.GetUIView<DungeonMapUI>();
        dungeonMapUI.LoadRoomData(dungeonSaveData.RoomDatas);
        dungeonMapUI.IsLoadData = true;
        
        
       
    }
}
