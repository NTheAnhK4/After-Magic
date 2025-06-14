

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UnityEngine;

public class CoinItem : ItemBase
{
    protected override async UniTask GainRewardAnim()
    {
        await base.GainRewardAnim();
        ObserverManager<GameEventType>.Notify(GameEventType.GainCoin, amount);
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        RewardPosition = new Vector3(-16, 9);
        minAmountGained = 10;
        maxAmountGained = 100;
        bossRewardMultiplier = 10;
    }
    
}
