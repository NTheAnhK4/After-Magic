using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UsingCardState : ICardState
{
    private CardManager cardManager;
    private Func<bool> getCanUseCard;

    public UsingCardState(CardManager cardManager, Func<bool> getCanUseCard)
    {
        this.cardManager = cardManager;
        this.getCanUseCard = getCanUseCard;
    }
    public UniTask OnEnter()
    {
        bool canUseCard = true;
        if (getCanUseCard != null) canUseCard = getCanUseCard.Invoke();
        cardManager.SetCardUsable(canUseCard);
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
