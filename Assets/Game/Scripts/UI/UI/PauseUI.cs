

using AudioSystem;

using Game.UI;
using SaveGame;
using UnityEngine;


public class PauseUI : UIView
{
    [SerializeField] private ButtonAnimBase settingBtn;
    [SerializeField] private ButtonAnimBase continueBtn;
    [SerializeField] private ButtonAnimBase giveUpBtn;

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Setting").GetComponent<ButtonAnimBase>();
        if (continueBtn == null) continueBtn = transform.Find("Continue").GetComponent<ButtonAnimBase>();
        if (giveUpBtn == null) giveUpBtn = transform.Find("Give up").GetComponent<ButtonAnimBase>();
    }

    private void OnEnable()
    {
        continueBtn.onClick += OnContinueBtnClick;
        giveUpBtn.onClick += OnGiveUpBtnClick;
        settingBtn.onClick += OnSettingBtnClick;
       
    }

    private async void OnContinueBtnClick() => await UIScreen.HideUI<PauseUI>();
    private async void OnSettingBtnClick() => await UIScreen.ShowAfterHide<SettingUI>();

    private void OnDisable()
    {
        continueBtn.onClick -= OnContinueBtnClick;
        giveUpBtn.onClick -= OnGiveUpBtnClick;
        settingBtn.onClick -= OnSettingBtnClick;
    }

    private async void OnGiveUpBtnClick()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(20);
        
        AchivementUI achivementUI = UIScreen.GetUIView<AchivementUI>();
        achivementUI.SetRedBtn("Exit", () =>
        {
            InventoryManager.Instance.MoveLootToInventory();
            if (SaveLoadSystem.Instance.GameData != null)
            {
                SaveLoadSystem.Instance.GameData.ExitDungeon();
                SaveLoadSystem.Instance.SaveGame();
            }
            SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
        });
        
        await UIScreen.ShowAfterHide<AchivementUI>();
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
    }
}
