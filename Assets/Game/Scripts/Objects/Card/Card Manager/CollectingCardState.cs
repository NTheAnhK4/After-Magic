
using System.Collections.Generic;
using AudioSystem;
using Cysharp.Threading.Tasks;

public class CollectingCardState : ICardState
{
   
   
    
    public async UniTask OnEnter()
    {
        CardManager.Instance.CurrentUsingCard = null;
        CardManager.Instance.DisableOtherCardsRayCast();
        List<UniTask> tasks = new List<UniTask>();

        
        foreach (Card card in CardManager.Instance.CardInHands)
        {
            tasks.Add(CollectingCard(card));
        }
      
       
        await UniTask.WhenAll(tasks);
        
        if (CardManager.Instance == null) return;
        
        if (CardManager.Instance.CollectingCardSound != null)
        {
            SoundManager.Instance.CreateSound().WithSoundData(CardManager.Instance.CollectingCardSound).Play();
        }
        foreach (Card card in CardManager.Instance.CardInHands)
        {
            CardManager.Instance.DisCardPile.Add(card.CardDataCtrl.PlayerCardData);
        }
      
        CardManager.Instance.CardInHands.Clear();
        //set discard pile count change
        ObserverManager<CardEventType>.Notify(CardEventType.DiscardPileCountChange, CardManager.Instance.DisCardPile.Count);

        await UniTask.Delay(500, DelayType.UnscaledDeltaTime);
        
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
