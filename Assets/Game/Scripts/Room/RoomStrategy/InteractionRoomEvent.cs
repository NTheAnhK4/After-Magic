using System;
using Game.UI;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Room Strategy/ Interaction Room", fileName = "Interaction Room")]
public class InteractionRoomEvent : RoomEventStrategy
{
    public GameObject UIPrefab;
    public override void OnEnter()
    {
        if (UIPrefab== null)
        {
            Debug.LogWarning("UIPrefab is null");
            return;
        }
        UIScreen.Instance.HideUIOnTop();
        UIScreen.Instance.ShowUI(UIPrefab);
    }
}