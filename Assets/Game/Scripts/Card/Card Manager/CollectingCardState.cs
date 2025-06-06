
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

public class CollectingCardState : ICardState
{
   
   
    
    public async UniTask OnEnter()
    {
        CardManager.Instance.CurrentUsingCard = null;
        CardManager.Instance.EnableAllCardsRayCast();
        List<UniTask> tasks = new List<UniTask>();

        List<Card> cards = new List<Card>(CardManager.Instance.CardInHands);
        foreach (Card card in cards)
        {
            tasks.Add(CollectingCard(card));
        }
      
       
        await UniTask.WhenAll(tasks);
        //set discard pile count change
        ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, CardManager.Instance.DisCardPile.Count);
   
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
