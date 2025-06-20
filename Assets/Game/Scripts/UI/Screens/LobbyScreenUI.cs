using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreenUI : UIScreen
{
    [SerializeField] private Button settingBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Top/Setting").GetComponent<Button>();
        AddUIView<SettingUI>();
    }

    private async void OnSettingBtnClick() => await ShowUI<SettingUI>();

    private void OnEnable()
    {
        settingBtn.onClick.AddListener(OnSettingBtnClick);
    }

    private void OnDisable()
    {
        settingBtn.onClick.RemoveAllListeners();
    }
}
