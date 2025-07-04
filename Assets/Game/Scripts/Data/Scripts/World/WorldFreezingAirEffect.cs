
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/World/World Effect/WorldFreezingAirEffect", fileName = "WorldFreezingAirEffect")]
public class WorldFreezingAirEffect : WorldEffect
{
    public Sprite frozenSprite;
    public Material frozenMaterial;
    public override void RegisterEnvent()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.FinishDistributeCard, FreezingRandomCard);
    }

    public override void UnRegisterEvent()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.FinishDistributeCard, FreezingRandomCard);
    }

    private void FreezingRandomCard(object param)
    {
        if (CardManager.Instance == null) return;
        List<Card> cardInHands = CardManager.Instance.CardInHands;
        if(cardInHands == null || cardInHands.Count == 0) return;
        int cardId = Random.Range(0, cardInHands.Count);

        cardInHands[cardId].CardDataCtrl.CardStrategy = null;
        cardInHands[cardId].CardDataCtrl.SetCardSurface(frozenSprite, frozenMaterial);
    }
}
