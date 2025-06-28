
using AudioSystem;

using Game.UI;
using UnityEngine;



public class LoseUI : UIView
{
    [SerializeField] private ButtonAnimBase defeatBtn;
    [SerializeField] private ButtonAnimBase continueBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (defeatBtn == null) defeatBtn = transform.Find("Defeat").GetComponent<ButtonAnimBase>();
        if (continueBtn == null) continueBtn = transform.Find("Continue").GetComponent<ButtonAnimBase>();
    }

    private void OnEnable()
    {
        defeatBtn.onClick += OnDefeatBtnClick;
        continueBtn.onClick += OnContinueBtnClick;
    }
    
    private void OnContinueBtnClick(){
    {
        InGameScreenUI inGameScreenUI = UIScreen as InGameScreenUI;
        inGameScreenUI.OrNull()?.OnRevivePlayer?.Invoke();
       
    }}

    private async void OnDefeatBtnClick()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(0);

        AchivementUI achivementUI = UIScreen.GetUIView<AchivementUI>();
        achivementUI.SetRedBtn("Exit", () => SceneLoader.Instance.LoadScene(GameConstants.LobbyScene));
       
        await UIScreen.ShowAfterHide<AchivementUI>();
    }

    private void OnDisable()
    {
        defeatBtn.onClick -= OnDefeatBtnClick;
        continueBtn.onClick -= OnContinueBtnClick;
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.StopAll);
        base.Show();
    }
}
