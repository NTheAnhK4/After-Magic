using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIView
{
    [Header("Buttons")] [SerializeField] 
    private Button takeAllBtn;
    [SerializeField] private Button skipRewardBtn;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (takeAllBtn == null) takeAllBtn = transform.Find("Take All").GetComponent<Button>();
        if (skipRewardBtn == null) skipRewardBtn = transform.Find("Skip Reward").GetComponent<Button>();
    }

   
    private void OnEnable()
    {
        skipRewardBtn.onClick.AddListener(ShowDungeonMapUI);
    }

    private void OnDisable()
    {
        skipRewardBtn.onClick.RemoveAllListeners();
    }

    private void CloseUI()
    {
        if(UIScreen != null) UIScreen.HideUI();
        else Hide();
    }

    private void ShowDungeonMapUI()
    {
        DungeonMapUI dungeonMapUI = ((InGameScreenUI)(UIScreen)).DungeonMapUI;
        dungeonMapUI.IsVirtualMap = false;
        ((InGameScreenUI)(UIScreen)).ShowAfterHide(dungeonMapUI);
    }
}
