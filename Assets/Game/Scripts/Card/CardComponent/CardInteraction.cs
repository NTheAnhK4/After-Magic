

using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteraction : CardComponent, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
   

    private Vector3 offset;
    private float zCoord;
  
    private bool CanUseCard() => CardManager.Instance.CurrentUsingCard == null || CardManager.Instance.CurrentUsingCard ==this;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanUseCard()) return;
        if (!card.CardDataCtrl.CanUseData()) return;
        
        InGameManager.Instance.SetTurn(GameStateType.UsingCard);
        CardManager.Instance.CurrentUsingCard = this;
        ObserverManager<CardEventType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardEventType.PlayerTarget : CardEventType.EnemyTarget, true);
        if (Camera.main != null) zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePoint = GetMouseWorldPosition();
        var cardTrf = transform;
        offset = cardTrf.position - mousePoint;
        cardTrf.rotation = Quaternion.Euler(Vector3.zero);
        cardTrf.localScale = new Vector3(.8f, .8f, 1);

        

    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!CanUseCard()) return;
        card.CardAnimation.SetSortingLayer(card.selectedLayer);
      
        Vector3 mousePoint = GetMouseWorldPosition();

        Vector3 newPosition = new Vector3((mousePoint.x + offset.x), mousePoint.y + offset.y, 0);
        transform.position = newPosition;

    }

    public void OnPointerUp(PointerEventData eventData)
    {
       
        ObserverManager<CardEventType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardEventType.PlayerTarget : CardEventType.EnemyTarget, false);
       
        if (card.CardAction.TryUseCard()) return;
        card.CardAnimation.ReturnHand();
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        if (Camera.main != null) return Camera.main.ScreenToWorldPoint(mousePoint);
        return Vector3.zero;
    }
}
