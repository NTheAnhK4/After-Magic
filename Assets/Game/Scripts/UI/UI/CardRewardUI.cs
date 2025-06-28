
using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;



public class CardRewardUI : UIView
{
    public PlayerCardListData PlayerCardListData;
   
    public GameObject cardOptionPrefab;
    
    public Action OnFinishChooseCard;
    [SerializeField] private Transform cardOptionHolder;

    
    private List<CardOption> currentCardOptions = new List<CardOption>();
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (cardOptionHolder == null) cardOptionHolder = transform.Find("Cards/View/Content");
    }

    public override void OnFinishedShow()
    {
        base.OnFinishedShow();
        ShowCards();
    }

    

    public async void ShowCards()
    {
        currentCardOptions.Clear();
        for (int i = 0; i < 3; ++i)
        {
            
            CardOption cardOption = PoolingManager.Spawn(cardOptionPrefab, cardOptionHolder).GetComponent<CardOption>();
            if (cardOption == null)
            {
                Debug.LogWarning("Don't have Card Option in cardOption prefab");
                continue;
            }
            cardOption.SetInteracble(false);
            cardOption.transform.SetSiblingIndex(i);
            if (PlayerCardListData == null)
            {
                Debug.LogWarning("Player Card List Data is null");
                return;
            }
            cardOption.SetCardData(PlayerCardListData.GetRandomCard());
            
            cardOption.CardRewardUI = this;
            currentCardOptions.Add(cardOption);
            await cardOption.OnShowCard();
        }

        SetCardInteracble(true);
    }

    public void SetCardInteracble(bool interacable)
    {
        foreach (CardOption cardOption  in currentCardOptions)
        {
            if(cardOption == null) continue;
            cardOption.SetInteracble(interacable);
        }
    }
    public async void DisableOtherCard(CardOption cardOption)
    {
        List<UniTask> uniTasks = new List<UniTask>();
        foreach (CardOption card in currentCardOptions)
        {
            if(card == null || card == cardOption) continue;
            uniTasks.Add(card.DisableCard()); 
        }

        await UniTask.WhenAll(uniTasks);
        currentCardOptions.Clear();
        await UIScreen.HideUI<CardRewardUI>(false, OnFinishChooseCard);
    }
}
