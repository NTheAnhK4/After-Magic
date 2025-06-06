
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DistributeCardState : ICardState
{
   
 

    private List<Vector3> cardPositions;
    private List<Vector3> cardRotations;
    
    public async UniTask OnEnter()
    {
        CardManager.Instance.CurrentUsingCard = null;
        CardManager.Instance.EnableAllCardsRayCast();
        
        CardManager.Instance.CardInHands.Clear();
        
       
        int cardCount = 5;


        if (CardManager.Instance.DrawPile.Count < cardCount)
        {
            await DisCardPileToDrawPile();
            ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, CardManager.Instance.DisCardPile.Count);
        }
        cardCount = Math.Min(cardCount, CardManager.Instance.DrawPile.Count);
        CardManager.Instance.ArrangeHand(cardCount, out cardPositions, out cardRotations);
        for (int i = 0; i < cardCount; ++i)
        {
          
            Card card = await SpawnCard(i);
            card.CardAnimation.SetSiblingIndex(cardCount - 1 - i);
            
        }
        ObserverManager<CardEventType>.Notify(CardEventType.DrawPileCountChange, CardManager.Instance.DrawPile.Count);
        //player turn
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }
    private async UniTask<Card> SpawnCard(int cardNumber)
    {
        int cardId = Random.Range(0, CardManager.Instance.DrawPile.Count);

        Card card = CardManager.Instance.DrawPile[cardId];
        card.gameObject.SetActive(true);
        
        CardManager.Instance.DrawPile.RemoveAt(cardId);
       
       
        card.CardAnimation.SelectCard();
        
        CardManager.Instance.CardInHands.Add(card);
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(card.transform.DOScale(1, .2f))
            .Join(card.transform.DOMove(cardPositions[cardNumber],.2f))
            .Join(card.transform.DORotate(cardRotations[cardNumber], .15f, RotateMode.Fast));
        
        await sequence.AsyncWaitForCompletion();
        card.CardAnimation.DeselectCard();
        return card;
    }

    private async UniTask DisCardPileToDrawPile()
    {
        List<Card> disCardPile = new List<Card>(CardManager.Instance.DisCardPile);
        if (disCardPile.Count == 0) return;

      

        List<UniTask> uniTasks = new List<UniTask>();
        foreach (Card disCard in CardManager.Instance.DisCardPile)
        {
            uniTasks.Add(EachCardToDrawPile(disCard));
        }

        await UniTask.WhenAll(uniTasks);
        CardManager.Instance.DrawPile.AddRange(disCardPile);
        CardManager.Instance.DisCardPile.Clear();
    }

    private async UniTask EachCardToDrawPile(Card card)
    {
        card.gameObject.SetActive(true);
     
        Sequence sequence = DOTween.Sequence();
        sequence.
            Append(card.transform.DORotate(new Vector3(0, 0, 45), .5f))
            .Join(card.transform.DOMove(CardManager.Instance.spawnPos, .65f));
        await sequence.AsyncWaitForCompletion();
        card.gameObject.SetActive(false);
    }

   

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
    
}
