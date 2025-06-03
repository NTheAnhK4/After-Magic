
using DG.Tweening;
using UnityEngine;


public class CardAnimation : CardComponent
{
    
    private Vector3 prePosition;
    private Quaternion preQuaternion;
    private Vector3 preScale;
   
    protected override void Awake()
    {
        base.Awake();
        SaveInitialTransform();
    }

    private void OnEnable()
    {
        SetSortingLayer(card.inHandLayer);
    }

    public void SaveInitialTransform()
    {
       
        var cardTransform = transform;
        prePosition = cardTransform.position;
        preQuaternion = cardTransform.rotation;
        preScale = cardTransform.localScale;
    }

    public void SetSortingLayer(string layer)
    {
        if (card.SortingGroup != null) card.SortingGroup.sortingLayerName = layer;
    }

    public void SetSortingOrder(int sortingValue)
    {
       
        if (card.SortingGroup != null) card.SortingGroup.sortingOrder = sortingValue;
        SaveInitialTransform();
    }

    public async void ReturnHand()
    {
       
        SetSortingLayer(card.inHandLayer);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(prePosition,.3f))
            .Join(transform.DORotate(preQuaternion.eulerAngles,.3f))
            .Join(transform.DOScale(preScale, .3f));
        await sequence.AsyncWaitForCompletion();
       
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }
}
