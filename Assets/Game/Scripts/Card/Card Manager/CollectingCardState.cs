
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class CollectingCardState : ICardState
{
   
   
    
    public async UniTask OnEnter()
    {
        CardManager.Instance.CurrentUsingCard = null;
       
        List<UniTask> tasks = new List<UniTask>();

        List<Card> cards = new List<Card>(CardManager.Instance.cards);
        foreach (Card card in cards)
        {
            tasks.Add(CollectingCard(card));
        }
      
       
        await UniTask.WhenAll(tasks);
       
   
        InGameManager.Instance.SetTurn(GameStateType.EnemyTurn);
       
       
    }
    private async UniTask CollectingCard(Card card)
    {
        if (card == null) return;
        await CardManager.Instance.CollectingCard(card, true);
    }
    public UniTask OnExit()
    {
        return UniTask.CompletedTask;
    }

    public void Update()
    {
    }
}
