
using DG.Tweening;
using UnityEngine;


public class CardAnimation : CardComponent
{
    
    private Vector3 prePosition;
    private Quaternion preQuaternion;
    private Vector3 preScale;
    private int siblingIndex = 0;
    protected override void Awake()
    {
        base.Awake();
        SaveInitialTransform();
    }

    

    public void SaveInitialTransform()
    {
       
        var cardTransform = transform;
        prePosition = cardTransform.position;
        preQuaternion = cardTransform.rotation;
        preScale = cardTransform.localScale;
    }

    public void SetSiblingIndex(int siblingID)
    {
        SaveInitialTransform();
        siblingIndex = siblingID;
        transform.SetSiblingIndex(siblingID);
    }

    public void SelectCard() => transform.SetAsLastSibling();
    public void DeselectCard() => transform.SetSiblingIndex(siblingIndex);
 
    public async void ReturnHand()
    {
       
        DeselectCard();
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(prePosition,.3f))
            .Join(transform.DORotate(preQuaternion.eulerAngles,.3f))
            .Join(transform.DOScale(preScale, .3f));
        await sequence.AsyncWaitForCompletion();
       
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }
}
