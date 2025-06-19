
using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : UIView
{
    [Header("Buttons")] 
    [SerializeField] private Button takeAllBtn;
    [SerializeField] private Button skipRewardBtn;
    [SerializeField] private Transform rewardHolder;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (takeAllBtn == null) takeAllBtn = transform.Find("Take All").GetComponent<Button>();
        if (skipRewardBtn == null) skipRewardBtn = transform.Find("Skip Reward").GetComponent<Button>();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
    }
    

    public override void Show()
    {
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
        skipRewardBtn.onClick.AddListener(SkipReward);
        takeAllBtn.onClick.AddListener(TakeAllReward);
        RewardManager.Instance.CurrentRewardEmpty += CurrentRewardEmpty;
    }

    private void OnDisable()
    {
        takeAllBtn.onClick.RemoveAllListeners();
        skipRewardBtn.onClick.RemoveAllListeners();
        RewardManager.Instance.CurrentRewardEmpty -= CurrentRewardEmpty;
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
        await UIScreen.ShowAfterHide<DungeonMapUI>(true);
       
    }

    private void CurrentRewardEmpty()
    {
        ShowDungeonMapUI();
    }
}
