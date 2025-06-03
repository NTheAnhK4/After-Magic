

using System;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class HealthUICtrl : ComponentBehavior
{
    [SerializeField] private Entity _entity;
    [SerializeField] private Image hpImg;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (hpImg == null) hpImg = transform.Find("HP Mask").Find("HP").GetComponent<Image>();
        if (_entity == null) _entity = transform.parent.GetComponentInParent<Entity>();
    }

    private void OnEnable()
    {
        if (_entity != null) _entity.OnHPChange += OnHPChange;
    }

    private void OnDisable()
    {
        if (_entity != null) _entity.OnHPChange -= OnHPChange;
    }

    private void OnHPChange()
    {
        hpImg.fillAmount = 1.0f *_entity.CurHP / _entity.MaxHP;
    }
}
