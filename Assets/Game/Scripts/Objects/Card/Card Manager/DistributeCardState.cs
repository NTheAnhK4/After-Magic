
using System;
using System.Collections.Generic;
using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using UnityEngine;
using Random = UnityEngine.Random;

public class DistributeCardState : ICardState
{
   
  


    private List<Vector3> cardPositions;
    private List<Vector3> cardRotations;
    private SoundBuilder soundBuilder;
    
    public async UniTask OnEnter()
    {
        
        
        CardManager.Instance.CurrentUsingCard = null;
        CardManager.Instance.DisableOtherCardsRayCast();
        
        CardManager.Instance.CardInHands.Clear();
        
       
        int cardCount = 5;
        
        
        //Get card from discard pile
        if (CardManager.Instance.DrawPile.Count < cardCount)
        {
            await DisCardPileToDrawPile();
            if (CardManager.Instance == null) return;
            
            if(CardManager.Instance.CollectingCardSound != null) SoundManager.Instance.CreateSound().WithSoundData(CardManager.Instance.CollectingCardSound).Play();
            ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, CardManager.Instance.DisCardPile.Count);
            await UniTask.Delay(500, DelayType.UnscaledDeltaTime);
        }
        
        cardCount = Math.Min(cardCount, CardManager.Instance.DrawPile.Count);

        //play distribute card sound
        if (CardManager.Instance.DistributeCardSound != null)
        {
            soundBuilder = SoundManager.Instance.CreateSound().WithSoundData(CardManager.Instance.DistributeCardSound);
            soundBuilder.Play();
        }
        
        //distribute card
        List<Card> cards = new List<Card>();
        CardManager.Instance.ArrangeHand(cardCount, out cardPositions, out cardRotations);
        for (int i = 0; i < cardCount; ++i)
        {
          
            Card card = await SpawnCard(i);
            if (CardManager.Instance == null) return;
            card.transform.SetAsFirstSibling();
            cards.Add(card);
            
        }

        for (int i = 0; i < cardCount; ++i)
        {
            cards[i].CardAnimation.SetSiblingIndex(cardCount - 1 - i);
        }
        
        ObserverManager<CardEventType>.Notify(CardEventType.DrawPileCountChange, CardManager.Instance.DrawPile.Count);
        await UniTask.Delay(200);
        if (CardManager.Instance == null) return;
        //player turn
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }
    private async UniTask<Card> SpawnCard(int cardNumber)
    {
       
        int cardId = Random.Range(0, CardManager.Instance.DrawPile.Count);

        PlayerCardData playerCardData = CardManager.Instance.DrawPile[cardId];

        Card card = PoolingManager.Spawn(CardManager.Instance.CardPrefab.gameObject, CardManager.Instance.spawnPos, Quaternion.Euler(CardManager.Instance.spawnRotation),
            CardManager.Instance.transform).GetComponent<Card>();
        card.gameObject.SetActive(false);
        card.CardDataCtrl.Init(playerCardData, false);
        card.SetUsable(false, true);
        card.transform.localScale = CardManager.Instance.spawnScale;
        
        card.gameObject.SetActive(true);
        
      
        CardManager.Instance.DrawPile.RemoveAt(cardId);
       
       
        card.CardAnimation.SelectCard();
        
        CardManager.Instance.CardInHands.Add(card);
        Sequence sequence = DOTween.Sequence();
     
        sequence.Append(card.transform.DOScale(1, .2f))
            .Join(card.transform.DOMove(cardPositions[cardNumber],.2f))
            .Join(card.transform.DORotate(cardRotations[cardNumber], .15f));
        
        await sequence.AsyncWaitForCompletion();
        
        return card;
    }

    private async UniTask DisCardPileToDrawPile()
    {
        if (CardManager.Instance.DisCardPile.Count == 0) return;
        
        List<UniTask> uniTasks = new List<UniTask>();
        foreach (PlayerCardData playerCardData in CardManager.Instance.DisCardPile)
        {
            Card card = PoolingManager.Spawn(CardManager.Instance.CardPrefab.gameObject, CardManager.Instance.despawnPos, Quaternion.Euler(CardManager.Instance.despawnRotation),
                CardManager.Instance.transform).GetComponent<Card>();
            card.gameObject.SetActive(false);
            card.CardDataCtrl.Init(playerCardData, false);
            card.SetUsable(false, true);
            card.transform.localScale = CardManager.Instance.despawnScale;
        
            card.gameObject.SetActive(true);
            uniTasks.Add(EachCardToDrawPile(card));
        }
        

        await UniTask.WhenAll(uniTasks);
        CardManager.Instance.DrawPile.AddRange(CardManager.Instance.DisCardPile);
        CardManager.Instance.DisCardPile.Clear();
    }

    private async UniTask EachCardToDrawPile(Card card)
    {
        if (card == null) return;
        card.gameObject.SetActive(true);
     
        Sequence sequence = DOTween.Sequence();
        sequence.
            Append(card.transform.DORotate(new Vector3(0, 0, 45), .5f))
            .Join(card.transform.DOMove(CardManager.Instance.spawnPos, .65f));
        await sequence.AsyncWaitForCompletion();
        card.OrNull()?.gameObject.SetActive(false);
    }

   

    public UniTask OnExit()
    {
        soundBuilder?.Stop();
        soundBuilder = null;
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
    
}
