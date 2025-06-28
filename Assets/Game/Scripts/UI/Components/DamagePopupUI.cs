using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamagePopupUI : ComponentBehavior
{
    [SerializeField] private TextMeshProUGUI damageTxt;
    private Color originalColor;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (damageTxt == null) damageTxt = GetComponent<TextMeshProUGUI>();
        originalColor = damageTxt.color;
        
    }
    

    public async void Show(int damage, Color color)
    {
        transform.position = transform.parent.TransformPoint(Vector3.zero);
        damageTxt.text = damage.ToString();
        damageTxt.color = originalColor;
        gameObject.SetActive(true);
        
        Sequence s = DOTween.Sequence();
        s.Join(damageTxt.DOColor(color, .3f));
        s.Join(transform.DOMoveY(transform.position.y + 1.5f, 0.5f).SetEase(Ease.OutQuad));
        
        s.Append(transform.DOMoveY(transform.position.y + 0.5f, 0.5f).SetEase(Ease.InQuad));
        s.Join(damageTxt.DOFade(0, 0.5f));
        s.OnComplete(() => gameObject.SetActive(false));
        await s.AsyncWaitForCompletion();
    }
    
}
