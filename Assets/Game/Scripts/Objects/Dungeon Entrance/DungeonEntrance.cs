using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonEntrance : ComponentBehaviour
{
    [SerializeField] private Image lockImg;
    public Button button;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (button == null) button = transform.GetComponent<Button>();
        if (lockImg == null) lockImg = transform.Find("LockImg").GetComponent<Image>();
    }

    public void UnLockDungeonEntrance()
    {
        lockImg.OrNull()?.gameObject.SetActive(false);
        button.interactable = true;
    }

    public void LockDungeonEntrance()
    {
        lockImg.OrNull()?.gameObject.SetActive(true);
        button.interactable = false;
    }
}
