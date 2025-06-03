
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DistributeCardState : ICardState
{
   
    private Vector3 spawnPos = new Vector3(-10, -9.5f,0);
    private Vector3 spawnScale = new Vector3(.8f, .8f, 1);

    private List<Vector3> cardPositions;
    private List<Vector3> cardRotations;
    
    public async UniTask OnEnter()
    {
        CardManager.Instance.CurrentUsingCard = null;
        
        CardManager.Instance.cards.Clear();
        
       
        int cardCount = 5;
        CardManager.Instance.ArrangeHand(cardCount, out cardPositions, out cardRotations);
        for (int i = 0; i < cardCount; ++i)
        {
            Card card = await SpawnCard(i);
            card.CardAnimation.SetSortingOrder(cardCount - i);
        }
        //player turn
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }
    private async UniTask<Card> SpawnCard(int cardNumber)
    {
        int cardId = Random.Range(0, CardManager.Instance.CardsAvailable.Count);
        Card card = PoolingManager.Spawn(CardManager.Instance.CardsAvailable[cardId].gameObject, spawnPos, default,CardManager.Instance.transform).GetComponent<Card>();
        
        
       
        card.transform.localScale = spawnScale;
        card.CardAnimation.SetSortingLayer(card.selectedLayer);
        
        CardManager.Instance.cards.Add(card);
        Sequence sequence = DOTween.Sequence();
        
        sequence.Append(card.transform.DOScale(1, .2f))
            .Join(card.transform.DOMove(cardPositions[cardNumber],.2f))
            .Join(card.transform.DORotate(cardRotations[cardNumber], .15f, RotateMode.Fast));
        
        await sequence.AsyncWaitForCompletion();
        card.CardAnimation.SetSortingLayer(card.inHandLayer);
        return card;
    }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
}
