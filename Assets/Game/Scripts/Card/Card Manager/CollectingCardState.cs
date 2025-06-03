
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CollectingCardState : ICardState
{
    private CardManager cardManager;
   

    public CollectingCardState(CardManager cardManager)
    {
        this.cardManager = cardManager;
    }
    public async UniTask OnEnter()
    {
        cardManager.SetCardUsable(false);
        List<UniTask> tasks = new List<UniTask>();
        for (int i = 0; i <cardManager.cards.Count; ++i)
        {
            tasks.Add(CollectingCard(i));
        }

        await UniTask.WhenAll(tasks);
        cardManager.cards.Clear();
        InGameManager.Instance.SetTurn(GameStateType.EnemyTurn);
    }
    private async UniTask CollectingCard(int cardId)
    {
        await cardManager.CollectingCard(cardManager.cards[cardId], true);
       
    }
    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
    }
}
