
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;


public class CardAction : CardComponent
{
    public bool TryUseCard()
    {
        var data = card.CardDataCtrl;
        var target = card.CardTargetHandler.CurrentTarget;
        if (target == null || !data.CanUseData()) return false;
        int manaCost = data.ManaCost;
    
        if (!InGameManager.Instance.CanUseMana(manaCost)) return false;
        InGameManager.Instance.TakeMana(manaCost);
        
        UseCard().Forget();
        return true;
    }

    private async UniTask UseCard()
    {
        var data = card.CardDataCtrl;
        var target = card.CardTargetHandler.CurrentTarget;
        if (target == null)
        {
            Debug.LogWarning("card target is null");
            return;
        }
        target.DeselectObject();
        
        if (!data.CardStrategy.AppliesToAlly)
        {
            //For enemy
            Player player = PlayerPartyManager.Instance.GetPlayer();
            player.EnemyTarget = target.GetComponent<Entity>();
            player.CardStrategy = data.CardStrategy;
            player.MustReachTarget = data.CardStrategy.MustReachTarget;
            ObserverManager<GameEventType>.Notify(GameEventType.PlayCard, card.CardDataCtrl.PlayerCardData.CardType);
        }
        else
        {
            //For Ally
            Entity entity = target.GetComponent<Entity>();
            if (entity == null) return;
            entity.CardStrategy = data.CardStrategy;
            ObserverManager<GameEventType>.Notify(GameEventType.PlayCard, card.CardDataCtrl.PlayerCardData.CardType);
        }
        await CardManager .Instance.CollectingCard(card, false);
    }
}
