
using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.UI;
using UnityEngine;


public class ItemCardSelection : ItemBase
{
   
    public CardRewardUI CardRewardUI;
    public override UniTask ShowReward(int amountGained = -1)
    {
        Color color = rewardImg.color;
        color.a = 1;
        rewardImg.color = color;
        return base.ShowReward();
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        minAmountGained = 3;
        maxAmountGained = 3;
        bossRewardMultiplier = 1;
        
    }

    public override UniTask GainReward()
    {
        ShowCardRewardUI();
        return UniTask.CompletedTask;
    }
    
    private  void ShowCardRewardUI()
    {
        CardRewardUI cardRewardUI = UIScreen.Instance.GetUIView<CardRewardUI>();
        cardRewardUI.OnFinishChooseCard = OnFinishChooseCard;
        UIScreen.Instance.ShowUI<CardRewardUI>();
    }

    protected override async UniTask GainRewardAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(rewardImg.DOFade(0,.3f))
            .Append(rewardBackground.DOFade(0,.6f))
            .SetUpdate(true);

        await seq.AsyncWaitForCompletion();
    }

    private async void OnFinishChooseCard()
    {
        await GainRewardAnim();
        Despawn();
    }
}
