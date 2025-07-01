using System.Collections;
using System.Collections.Generic;
using System.Persistence;
using Game.UI;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Dungeon/Room Strategy/ Door Room", fileName = "Door Room")]
public class DoorRoomEvent : RoomEventStrategy
{
    public override async void OnEnter()
    {
       
        int currentDepth = InGameManager.Instance.CurrentDepth;
        int maxDepth = InGameManager.Instance.MaxDepth;

        AchivementUI achivementUI = UIScreen.Instance.GetUIView<AchivementUI>();
        if (currentDepth == maxDepth)
        {
            achivementUI.SetBlueBtn("Stay",OnStay, 0);
            achivementUI.SetGreenBtn("Exit", OnExit,1);
        }
        else
        {
            achivementUI.SetRedBtn("Leave", OnLeave,0);
            achivementUI.SetBlueBtn("Stay", OnStay,1);
            achivementUI.SetGreenBtn("Go Deep", OnGoDeep,2);
        }
        await UIScreen.Instance.ShowAfterHide<AchivementUI>();
      
       
    }

    private async void OnStay()
    {
      
        await UIScreen.Instance.HideUI<AchivementUI>();

        DungeonMapUI dungeonMapUI = UIScreen.Instance.GetUIView<DungeonMapUI>();
        dungeonMapUI.IsVirtualMap = false;
        await UIScreen.Instance.ShowUI<DungeonMapUI>();
        
       
    }

    private void OnExit()
    {
        InventoryManager.Instance.MoveLootToInventory();
        if (SaveLoadSystem.Instance.gameData != null)
        {
            SaveLoadSystem.Instance.gameData.ExitDungeon();
            SaveLoadSystem.Instance.SaveGame();
        }
        SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
    }

    private void OnLeave()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(20);
        InventoryManager.Instance.MoveLootToInventory();
        if (SaveLoadSystem.Instance.gameData != null)
        {
            SaveLoadSystem.Instance.gameData.ExitDungeon();
            SaveLoadSystem.Instance.SaveGame();
        }
        SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
        
    }

    private async void OnGoDeep()
    {
        InGameManager.Instance.CurrentDepth++;

        await UIScreen.Instance.ShowPanel();
        
        
        DungeonMapUI dungeonMapUI = UIScreen.Instance.GetUIView<DungeonMapUI>();
        dungeonMapUI.RoomUISpawner.ResetRoom();
        dungeonMapUI.IsShown = false;
        dungeonMapUI.IsVirtualMap = false;
        dungeonMapUI.IsLoadData = false;
        
        await UIScreen.Instance.HideUI<AchivementUI>(true);
        
        await UIScreen.Instance.ShowUI<DungeonMapUI>();
        
       
    }
}
