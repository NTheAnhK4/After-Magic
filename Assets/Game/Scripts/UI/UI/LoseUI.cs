

using AudioSystem;

using Game.UI;
using SaveGame;
using UnityEngine;



public class LoseUI : UIView
{
    [SerializeField] private ButtonAnimBase defeatBtn;
    [SerializeField] private ButtonAnimBase continueBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        Transform buttonsHolder = transform.Find("Buttons");
        if (defeatBtn == null) defeatBtn = buttonsHolder.Find("Defeat").GetComponent<ButtonAnimBase>();
        if (continueBtn == null) continueBtn = buttonsHolder.Find("Continue").GetComponent<ButtonAnimBase>();
    }

    private void OnEnable()
    {
        defeatBtn.onClick += OnDefeatBtnClick;
        continueBtn.onClick += OnContinueBtnClick;
    }
    
    private async void OnContinueBtnClick(){
    {
        
        int dungeonLootCoin = InventoryManager.Instance.GetAmountFromLoot(ItemType.Coin);
        if (dungeonLootCoin >= 100)
        {
            InventoryManager.Instance.SetAmountFromLoot(ItemType.Coin, dungeonLootCoin - 100);
            ObserverManager<GameEventType>.Notify(GameEventType.ChanegCoin);
        }
        else
        {
            int equippedCoin = InventoryManager.Instance.GetAmountFromEquippedItems(ItemType.Coin);
            if (equippedCoin < 100 - dungeonLootCoin) return;
            InventoryManager.Instance.SetAmountFromLoot(ItemType.Coin,0);
            InventoryManager.Instance.SetAmountFromEquippedItems(ItemType.Coin,equippedCoin - 100 + dungeonLootCoin);
            ObserverManager<GameEventType>.Notify(GameEventType.ChanegCoin);
        }

        await UIScreen.HideUI<LoseUI>(true, () => InGameManager.Instance.RevivePlayer());
        
        
       
    }}

    private async void OnDefeatBtnClick()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(0);

        AchivementUI achivementUI = UIScreen.GetUIView<AchivementUI>();
        achivementUI.SetRedBtn("Exit", () =>
        {
            if (SaveLoadSystem.Instance.GameData != null)
            {
                SaveLoadSystem.Instance.GameData.ExitDungeon();
                SaveLoadSystem.Instance.SaveGame();
            }
            SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
        });
       
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
        int cointAmount = InventoryManager.Instance.GetAmountFromEquippedItems(ItemType.Coin) + InventoryManager.Instance.GetAmountFromLoot(ItemType.Coin);
        continueBtn.OrNull()?.gameObject.SetActive(cointAmount >= 100);
        base.Show();
    }
}
