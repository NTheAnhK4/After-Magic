using UnityEngine;

public class RoomUIComponent : ComponentBehavior
{
    [SerializeField] protected DungeonMapUI dungeonMapUI;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (dungeonMapUI == null) dungeonMapUI = GetComponent<DungeonMapUI>();
    }
}