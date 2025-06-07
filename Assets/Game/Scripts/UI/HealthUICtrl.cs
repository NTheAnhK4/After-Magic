

using System;
using StateMachine;
using UnityEngine;
using UnityEngine.UI;

public class HealthUICtrl : ComponentBehavior
{
    [SerializeField] private Entity _entity;
    [SerializeField] private Image hpImg;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (hpImg == null) hpImg = transform.Find("HP Mask").Find("HP").GetComponent<Image>();
        if (_entity == null) _entity = transform.parent.GetComponentInParent<Entity>();
    }

    protected override void Awake()
    {
        base.Awake();
        if (_entity != null) _entity.OnHPChange += OnHPChange;
    }

  
    private void OnHPChange()
    {
        hpImg.fillAmount = 1.0f *_entity.CurHP / _entity.MaxHP;
    }
}
