
using System;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIView
{
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button giveUpBtn;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Setting").GetComponent<Button>();
        if (continueBtn == null) continueBtn = transform.Find("Continue").GetComponent<Button>();
        if (giveUpBtn == null) giveUpBtn = transform.Find("Give up").GetComponent<Button>();
    }

    private void OnEnable()
    {
        continueBtn.onClick.AddListener(() => UIScreen.HideUI());
        giveUpBtn.onClick.AddListener(() => ((InGameScreenUI)UIScreen).OnShowAchivement?.Invoke());
        settingBtn.onClick.AddListener(() => ((InGameScreenUI)UIScreen).OnShowSetting?.Invoke());
    }

    private void OnDisable()
    {
        continueBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.RemoveAllListeners();
        settingBtn.onClick.RemoveAllListeners();
    }
}
