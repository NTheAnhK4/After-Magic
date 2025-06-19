
using Game.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Dungeon/Room Strategy/ Interaction Room", fileName = "Interaction Room")]
public class InteractionRoomEvent : RoomEventStrategy
{
    public GameObject UIPrefab;
    public override async void OnEnter()
    {
        if (UIPrefab== null)
        {
            Debug.LogWarning("UIPrefab is null");
            return;
        }

        await UIScreen.Instance.HideUI<AchivementUI>();
        
        await UIScreen.Instance.ShowUI(UIPrefab);
    }
}