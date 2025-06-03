using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UsingCardState : ICardState
{
    
    private Func<bool> getCanUseCard;

    public UsingCardState(Func<bool> getCanUseCard)
    {
        this.getCanUseCard = getCanUseCard;
    }
    public async UniTask OnEnter()
    {
        bool canUseCard = true;
        if (getCanUseCard != null) canUseCard = getCanUseCard.Invoke();
        await UniTask.Delay(250);
        if (canUseCard) CardManager.Instance.CurrentUsingCard = null;
        
    }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
}
