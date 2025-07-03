using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEntrance : ComponentBehaviour
{
    public Button button;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (button == null) button = transform.GetComponent<Button>();
    }

    public void UnLockDungeonEntrance()
    {
        button.interactable = true;
    }

    public void LockDungeonEntrance()
    {
        button.interactable = false;
    }
}
