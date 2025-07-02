using UnityEngine;
using UnityEngine.Serialization;

public class RoomUIComponent : ComponentBehaviour
{
    private DungeonMapUI dungeonMapUI;

    public DungeonMapUI DungeonMapUI
    {
        get
        {
            if (dungeonMapUI == null) dungeonMapUI = GetComponent<DungeonMapUI>();
            return dungeonMapUI;
        }
        set => dungeonMapUI = value;
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (DungeonMapUI == null) DungeonMapUI = GetComponent<DungeonMapUI>();
    }
}