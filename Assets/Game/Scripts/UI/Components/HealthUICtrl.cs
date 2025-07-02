

using System;
using BrokerChain;
using StateMachine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUICtrl : ComponentBehaviour
{
   
    [SerializeField] private Image hpImg;
    [SerializeField] private TextMeshProUGUI hpTxt;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (hpImg == null) hpImg = transform.Find("HP Mask").Find("HP").GetComponent<Image>();
        if (hpTxt == null) hpTxt = GetComponentInChildren<TextMeshProUGUI>();
    }

   

    public void Init(EntityStats entityStats)
    {
        entityStats.OnHPChange += OnHPChange;
    }

  
    private void OnHPChange(int curHP, int maxHP)
    {
        hpImg.fillAmount = 1.0f * curHP / maxHP;
        if (hpTxt == null) hpTxt = GetComponentInChildren<TextMeshProUGUI>();
        if (hpTxt == null)
        {
            Debug.Log("Can not find hptxt ");
            return;
        }

        hpTxt.text = curHP.ToString() + " / " + maxHP.ToString();
    }
}
