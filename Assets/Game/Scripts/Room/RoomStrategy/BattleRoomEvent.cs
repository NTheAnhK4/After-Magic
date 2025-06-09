
using Game.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Room Strategy/ Battle Room", fileName = "Battle Room")]
public class BattleRoomEvent : RoomEventStrategy
{
    public override void OnEnter()
    {
        UIScreen.Instance.HideUI<DungeonMapUI>();
        InGameManager.Instance.PlayGame();
    }
}
