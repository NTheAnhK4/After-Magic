
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class DistributeCardState : ICardState
{
    private CardManager cardManager;
    private Vector3 spawnPos = new Vector3(-10, -9.5f,0);
    private Vector3 spawnScale = new Vector3(.8f, .8f, 1);

    private List<Vector3> cardPositions;
    private List<Vector3> cardRotations;
    public DistributeCardState(CardManager cardManager)
    {
        this.cardManager = cardManager;
    }
    public async UniTask OnEnter()
    {
        cardManager.SetCardUsable(false);
        cardManager.cards.Clear();
        int cardCount = 5;
        cardManager.ArrangeHand(cardCount, out cardPositions, out cardRotations);
        for (int i = 0; i < cardCount; ++i)
        {
            await SpawnCard(i);
           cardManager.cards[i].CardAnimation.SetSortingOrder(cardCount - i);
        }
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
    }
    private async UniTask SpawnCard(int cardNumber)
    {
        int cardId = Random.Range(0, cardManager.CardsAvailable.Count);
        Card card = PoolingManager.Spawn(cardManager.CardsAvailable[cardId].gameObject, spawnPos, default,cardManager.transform).GetComponent<Card>();
        card.CardManager = cardManager;
        card.transform.localScale = spawnScale;
        card.CardAnimation.SetSortingLayer(card.selectedLayer);
        cardManager.cards.Add(card);
        Sequence sequence = DOTween.Sequence();
       
        
        sequence.Append(card.transform.DOScale(1, .5f))
            .Join(card.transform.DOMove(cardPositions[cardNumber],.5f))
            .Join(card.transform.DORotate(cardRotations[cardNumber], .5f, RotateMode.Fast));
        
        await UniTask.WaitUntil(() => !sequence.IsActive());
        card.CardAnimation.SetSortingLayer(card.inHandLayer);
    }

    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
        
    }
}
