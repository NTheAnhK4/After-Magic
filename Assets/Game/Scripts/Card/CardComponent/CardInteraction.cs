

using UnityEngine;

public class CardInteraction : CardComponent
{
   

    private Vector3 offset;
    private float zCoord;
    private bool hasDragged;

    private void OnMouseDown()
    {
        if (!card.CanUseCard) return;
        if (!card.CardDataCtrl.CanUseData()) return;
        InGameManager.Instance.SetTurn(GameStateType.UsingCard);
        
        ObserverManager<CardTargetType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardTargetType.Player : CardTargetType.Enemy, true);
        if (Camera.main != null) zCoord = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePoint = GetMouseWorldPosition();
        var cardTrf = transform;
        offset = cardTrf.position - mousePoint;
        cardTrf.rotation = Quaternion.Euler(Vector3.zero);
        cardTrf.localScale = new Vector3(.8f, .8f, 1);

        card.CanUseCard = true;

    }

    private void OnMouseDrag()
    {
        if(!card.CanUseCard) return;
        card.CardAnimation.SetSortingLayer(card.selectedLayer);
        hasDragged = true;
        Vector3 mousePoint = GetMouseWorldPosition();
        transform.position = mousePoint + offset;
    }

    private void OnMouseUp()
    {
        ObserverManager<CardTargetType>.Notify(card.CardDataCtrl.CardStrategy.AppliesToAlly ? CardTargetType.Player : CardTargetType.Enemy, false);
        if (!card.CanUseCard) return;
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
