

using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : CardComponent,  IDragHandler, IBeginDragHandler, IEndDragHandler
{

    private RectTransform rectTransform;
   
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
    }

   
 
  
    public void OnBeginDrag(PointerEventData eventData)
    {
       
      
        if (!card.CardDataCtrl.CanUseData()) return;
        ObserverManager<CardEventType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardEventType.PlayerTarget : CardEventType.EnemyTarget, true);
        transform.localScale = new Vector3(.8f, .8f, 1);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        CardManager.Instance.CurrentUsingCard = this.card;
       
        
        InGameManager.Instance.SetTurn(GameStateType.UsingCard);
        
       
        card.CardAnimation.SelectCard();
    }
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / CardManager.Instance.Canvas.scaleFactor;
    }


    public void OnEndDrag(PointerEventData eventDaata)
    {
        ObserverManager<CardEventType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardEventType.PlayerTarget : CardEventType.EnemyTarget, false);
        
        if (card.CardAction.TryUseCard()) return;
        card.CardAnimation.ReturnHand();
       
    }

   
}
