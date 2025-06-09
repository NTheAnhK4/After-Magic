
using System;
using System.Collections.Generic;

using Cysharp.Threading.Tasks;
using Game.UI;
using UnityEngine;
using Random = UnityEngine.Random;


public class CardRewardUI : UIView
{
    public List<PlayerCardData> PlayerCardDatas = new List<PlayerCardData>();
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
            int cardID = Random.Range(0, PlayerCardDatas.Count);
            CardOption cardOption = PoolingManager.Spawn(cardOptionPrefab, cardOptionHolder).GetComponent<CardOption>();
            if (cardOption == null)
            {
                Debug.LogWarning("Don't have Card Option in cardOption prefab");
                continue;
            }
            cardOption.SetInteracble(false);
            cardOption.transform.SetSiblingIndex(i);
            cardOption.SetCardData(PlayerCardDatas[cardID]);
            
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
        UIScreen.HideUI<CardRewardUI>(false, OnFinishChooseCard);
    }
}
