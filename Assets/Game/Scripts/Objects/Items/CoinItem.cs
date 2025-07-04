

using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UnityEngine;

public class CoinItem : ItemBase
{
   

    public override void LoadComponent()
    {
        base.LoadComponent();
        RewardPosition = new Vector3(-16, 9);
        minAmountGained = 10;
        maxAmountGained = 100;
        bossRewardMultiplier = 10;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        itemType = ItemType.Coin;
    }

    protected override bool CanTakeRewardToLoot() => true;
}
