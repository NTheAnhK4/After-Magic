
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;


public class CardAnimation : CardComponent
{

    [SerializeField] private List<Image> _images;
    private Vector3 prePosition;
    private Quaternion preQuaternion;
    private Vector3 preScale;
    private int siblingIndex = 0;
    private Tween tween;
   

    public override void LoadComponent()
    {
        base.LoadComponent();
        _images = GetComponentsInChildren<Image>().ToList();
    }

    private void OnEnable()
    {
        SaveInitialTransform();
        SetColor(true);
    }

    public void SetColor(bool isWhite)
    {
        foreach (Image img in _images)
        {
            img.color = isWhite ? Color.white : Color.gray;
        }
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
        tween = sequence;
        await sequence.AsyncWaitForCompletion();
       
        InGameManager.Instance.SetTurn(GameStateType.PlayerTurn);
       
    }

    private void OnDisable()
    {
        tween?.Kill();
    }
}
