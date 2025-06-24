using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScreenUI : UIScreen
{
    [SerializeField] private ButtonAnimBase settingBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Top/Setting").GetComponent<ButtonAnimBase>();
        AddUIView<SettingUI>();
    }

    private async void OnSettingBtnClick() => await ShowUI<SettingUI>();

    private void OnEnable()
    {
        settingBtn.onClick += OnSettingBtnClick;
    }

    private void Start()
    {
        MusicManager.Instance.PlayMusic(MusicType.Lobby);
    }

    private void OnDisable()
    {
        settingBtn.onClick -= OnSettingBtnClick;
        
    }
}
