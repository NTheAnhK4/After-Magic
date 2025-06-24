
using AudioSystem;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIView
{
    [Header("Buttons")] 
    [SerializeField] private ButtonAnimBase takeAllBtn;
    [SerializeField] private ButtonAnimBase skipRewardBtn;
    [SerializeField] private Transform rewardHolder;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (takeAllBtn == null) takeAllBtn = transform.Find("Take All").GetComponent<ButtonAnimBase>();
        if (skipRewardBtn == null) skipRewardBtn = transform.Find("Skip Reward").GetComponent<ButtonAnimBase>();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
    }
    

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.StopAll);
        base.Show();
        CanvasGroup.interactable = false;
    }

    public override async void OnFinishedShow()
    {
        base.OnFinishedShow();
        await RewardManager.Instance.ShowRewards(rewardHolder);
        CanvasGroup.interactable = true;
    }

    private void OnEnable()
    {
        skipRewardBtn.onClick += SkipReward;
        takeAllBtn.onClick += TakeAllReward;
        RewardManager.Instance.CurrentRewardEmpty += CurrentRewardEmpty;
    }

    private void OnDisable()
    {
        skipRewardBtn.onClick -= SkipReward;
        takeAllBtn.onClick -= TakeAllReward;
        if(RewardManager.Instance != null) RewardManager.Instance.CurrentRewardEmpty -= CurrentRewardEmpty;
    }

    private void TakeAllReward()
    {
        CanvasGroup.interactable = false;
        RewardManager.Instance.TakeAllReward();
    }

    private void SkipReward()
    {
        CanvasGroup.interactable = false;
        RewardManager.Instance.SkipReward();
    }

    private async void ShowDungeonMapUI()
    {
        DungeonMapUI dungeonMapUI = UIScreen.GetUIView<DungeonMapUI>();
        
        dungeonMapUI.IsVirtualMap = false;
        await UIScreen.ShowAfterHide<DungeonMapUI>();
       
    }

    private void CurrentRewardEmpty()
    {
        ShowDungeonMapUI();
    }
}
