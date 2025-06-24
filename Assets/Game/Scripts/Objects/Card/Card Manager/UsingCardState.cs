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
    public UniTask OnEnter()
    {
        bool canUseCard = true;
        if (getCanUseCard != null) canUseCard = getCanUseCard.Invoke();
        CardManager.Instance.CurrentUsingCard = null;
        
        if (canUseCard) CardManager.Instance.EnableAllCardsRayCast();
        else CardManager.Instance.DisableOtherCardsRayCast();

        return UniTask.CompletedTask;
    }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
}
