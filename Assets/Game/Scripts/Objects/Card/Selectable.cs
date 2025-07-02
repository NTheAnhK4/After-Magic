using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Selectable : ComponentBehaviour
{
    private SpriteRenderer aim;
    private Color originalColor;
    private Vector3 originalScale;
    private Tween tween;

    private CardEventType _cardEventType;
    private Action<object> OnToggleAnimAction;
  
    private void OnEnable()
    {
        OnToggleAnimAction = param => ToggleAim((bool)param);
        ObserverManager<CardEventType>.Attach(_cardEventType, OnToggleAnimAction);
    }

    private void OnDisable()
    {
        ObserverManager<CardEventType>.Detach(_cardEventType, OnToggleAnimAction);
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (aim == null)
        {
            aim = transform.Find("Aim").GetComponent<SpriteRenderer>();
            originalColor = aim.color;
            aim.gameObject.SetActive(false);
            originalScale = aim.transform.localScale;
        }

        _cardEventType = gameObject.tag.Equals("Player") ? CardEventType.PlayerTarget : CardEventType.EnemyTarget;
       
    }

    private void ToggleAim(bool isSetActive)
    {
        aim.color = originalColor;
        aim.gameObject.SetActive(isSetActive);
    }
    public void SelectObject()
    {
        aim.color = Color.red;
        aim.transform.localScale = originalScale * 1.2f; 
        tween = aim.transform.DOScale(originalScale * 1.5f, .5f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void DeselectObject()
    {
        tween?.Kill();
        aim.transform.localScale = originalScale;
        aim.color = originalColor;
    }

    
}
