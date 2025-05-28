using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Selectable : ComponentBehavior
{
    private SpriteRenderer aim;
    private Color originalColor;
    private Vector3 originalScale;
    private Tween tween;

    private void Start()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.UsingCard, param => DisplayAim());
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn,param => HideAnim());
    }

    
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (aim == null)
        {
            aim = transform.Find("Aim").GetComponent<SpriteRenderer>();
            originalColor = aim.color;
            aim.gameObject.SetActive(false);
            originalScale = aim.transform.localScale;
        }
    }

    private void DisplayAim()
    {
        aim.color = originalColor;
       
        aim.gameObject.SetActive(true);
    }

    private void HideAnim()
    {
        aim.gameObject.SetActive(false);
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
