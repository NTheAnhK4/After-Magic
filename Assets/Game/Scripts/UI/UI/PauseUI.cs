
using System;
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : UIView
{
    [SerializeField] private Button settingBtn;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button giveUpBtn;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Setting").GetComponent<Button>();
        if (continueBtn == null) continueBtn = transform.Find("Continue").GetComponent<Button>();
        if (giveUpBtn == null) giveUpBtn = transform.Find("Give up").GetComponent<Button>();
    }

    private void OnEnable()
    {
        continueBtn.onClick.AddListener(() => UIScreen.HideUI<PauseUI>().Forget());
        giveUpBtn.onClick.AddListener(OnGiveUpBtnClick);
        settingBtn.onClick.AddListener(() => UIScreen.ShowAfterHide<SettingUI>().Forget());
    }

    private void OnDisable()
    {
        continueBtn.onClick.RemoveAllListeners();
        giveUpBtn.onClick.RemoveAllListeners();
        settingBtn.onClick.RemoveAllListeners();
    }

    private async void OnGiveUpBtnClick()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(20);
        UIScreen.GetUIView<AchivementUI>().SetLoseAchiveMent();
        await UIScreen.ShowAfterHide<AchivementUI>();
    }
    
}
