

using UnityEngine;

public class CardInteraction : CardComponent
{
   

    private Vector3 offset;
    private float zCoord;
    private bool hasDragged;
    private bool CanUseCard() => CardManager.Instance.CurrentUsingCard == null || CardManager.Instance.CurrentUsingCard ==this;
    private void OnMouseDown()
    {
        if (!CanUseCard()) return;
        if (!card.CardDataCtrl.CanUseData()) return;
        
        InGameManager.Instance.SetTurn(GameStateType.UsingCard);
        CardManager.Instance.CurrentUsingCard = this;
        ObserverManager<CardTargetType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardTargetType.Player : CardTargetType.Enemy, true);
        if (Camera.main != null) zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePoint = GetMouseWorldPosition();
        var cardTrf = transform;
        offset = cardTrf.position - mousePoint;
        cardTrf.rotation = Quaternion.Euler(Vector3.zero);
        cardTrf.localScale = new Vector3(.8f, .8f, 1);

        

    }

    private void OnMouseDrag()
    {
        if(!CanUseCard()) return;
        card.CardAnimation.SetSortingLayer(card.selectedLayer);
        hasDragged = true;
        Vector3 mousePoint = GetMouseWorldPosition();
        mousePoint.z = 0;
        transform.position = mousePoint + offset;
    }

    private void OnMouseUp()
    {
        if (!CanUseCard()) return;
        ObserverManager<CardTargetType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardTargetType.Player : CardTargetType.Enemy, false);
       
        if (card.CardAction.TryUseCard()) return;
        if (hasDragged)
        {
            card.CardAnimation.ReturnHand();
            hasDragged = false;
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        if (Camera.main != null) return Camera.main.ScreenToWorldPoint(mousePoint);
        return Vector3.zero;
    }
}
