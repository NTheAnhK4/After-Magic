
using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;

using UnityEngine.UI;

public class LoseUI : UIView
{
    [SerializeField] private Button defeatBtn;
    [SerializeField] private Button continueBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (defeatBtn == null) defeatBtn = transform.Find("Defeat").GetComponent<Button>();
        if (continueBtn == null) continueBtn = transform.Find("Continue").GetComponent<Button>();
    }

    private void OnEnable()
    {
        defeatBtn.onClick.AddListener(OnDefeatBtnClick);
        continueBtn.onClick.AddListener(() => ((InGameScreenUI)UIScreen).OnRevivePlayer?.Invoke());
    }

    private async void OnDefeatBtnClick()
    {
        InventoryManager.Instance.SetDungeonLootPercentage(0);
        UIScreen.GetUIView<AchivementUI>().SetLoseAchiveMent();
        await UIScreen.ShowAfterHide<AchivementUI>();
    }

    private void OnDisable()
    {
        defeatBtn.onClick.RemoveAllListeners();
    }

   
    

   
}
