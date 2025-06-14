using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

public class RewardManager : Singleton<RewardManager>
{
    public List<GameObject> rewards;

    private List<ItemBase> currentRewards = new List<ItemBase>();

    private Stack<ItemBase> rewardStack = new Stack<ItemBase>();

    public Action CurrentRewardEmpty;

    private bool isTakeAllReward;

    public async UniTask ShowRewards(Transform parent)
    {
        isTakeAllReward = false;
        for (int i = 0; i < 3; ++i)
        {
            int rewardID = i % 2;
            ItemBase itemBase = PoolingManager.Spawn(rewards[rewardID], parent).GetComponent<ItemBase>();
           
            itemBase.SetInteracable(true);
            itemBase.transform.SetSiblingIndex(i);
            currentRewards.Add(itemBase);
            itemBase.transform.localScale = Vector3.one;
          
            await itemBase.ShowReward();
        }
    }

 
    //use for win UI
    public void TakeAllReward()
    {
        rewardStack.Clear();
        for (int i = currentRewards.Count - 1; i >= 0; --i)
        {
            ItemBase itemBase = currentRewards[i];
            if(itemBase == null) continue;
            rewardStack.Push(itemBase);
        }
      
        //Start gain reward
        if (rewardStack.Count > 0)
        {
            isTakeAllReward = true;
            TakeEachReward();
        }
        else currentRewards.Clear();
    }

    public void SkipReward()
    {
        foreach (ItemBase rewardBase in currentRewards)
        {
            if(rewardBase == null) continue;
            PoolingManager.Despawn(rewardBase.gameObject);
        }
        currentRewards.Clear();
        CurrentRewardEmpty?.Invoke();
    }
    //Gain each reward
    public async void TakeEachReward()
    {
        if (!isTakeAllReward) return;
        if (rewardStack.Count <= 0)
        {
            currentRewards.Clear();
            return;
        }

        ItemBase itemBase = rewardStack.Pop();
         await itemBase.GainReward();
    }

    public void RemoveCurrentReward(ItemBase itemBase)
    {
        currentRewards.Remove(itemBase);
        if(currentRewards.Count == 0) CurrentRewardEmpty?.Invoke();
    }
}
