using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CollectingCardState : ICardState
{
    private CardManager cardManager;
    private Vector3 despawnPos = new Vector3(14, -8);
    private Vector3 despawnScale = new Vector3(.4f, .4f, 1);

    public CollectingCardState(CardManager cardManager)
    {
        this.cardManager = cardManager;
    }
    public async UniTask OnEnter()
    {
        List<UniTask> tasks = new List<UniTask>();
        for (int i = 0; i <cardManager.cards.Count; ++i)
        {
            tasks.Add(CollectingCard(i));
        }

        await UniTask.WhenAll(tasks);
        cardManager.cards.Clear();
        GameManager.Instance.TakeTurn();
    }
    private async UniTask CollectingCard(int cardId)
    {
        Transform card = cardManager.cards[cardId].transform;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(card.DOScale(despawnScale, .5f))
            .Join(card.DOMove(despawnPos, .75f))
            .Join(card.DORotate(new Vector3(0,0,225),.7f));
        
        await UniTask.WaitUntil(() => !sequence.IsActive());
        
        PoolingManager.Despawn(card.gameObject);

    }
    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
    }
}
