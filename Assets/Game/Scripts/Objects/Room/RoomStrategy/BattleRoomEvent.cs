
using Game.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dungeon/Room Strategy/ Battle Room", fileName = "Battle Room")]
public class BattleRoomEvent : RoomEventStrategy
{
    public override async void OnEnter()
    {
       
        await UIScreen.Instance.HideUI<DungeonMapUI>(true);
        
        InGameManager.Instance.PlayGame();
    }
}
