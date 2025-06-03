
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;


public class CardAction : CardComponent
{
    public bool TryUseCard()
    {
        var data = card.CardDataCtrl;
        var target = card.CardTargetHandler.CurrentTarget;
        if (target == null || !data.CanUseData() || !card.CardManager) return false;
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
            Player player = PlayerPartyManager.Instance.GetPlayer();
            player.EnemyTarget = target.GetComponent<Entity>();
            player.CardStrategy = data.CardStrategy;
            player.MustReachTarget = data.CardStrategy.MustReachTarget;
        }
        else
        {
            Entity entity = target.GetComponent<Entity>();
            if (entity == null) return;
            entity.CardStrategy = data.CardStrategy;
        }
        await card.CardManager.CollectingCard(card, false);
    }
}
