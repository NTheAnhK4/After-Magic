
using System;
using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public class AchivementUI : UIView
{
   
    [SerializeField] private Transform rewardHolder;
    private List<ItemBase> items = new List<ItemBase>();
    [SerializeField] private Button redBtn;
    [SerializeField] private Button greenBtn;
    [SerializeField] private Button blueBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
        redBtn = transform.Find("Buttons/Red").GetComponent<Button>();
        greenBtn = transform.Find("Buttons/Green").GetComponent<Button>();
        blueBtn = transform.Find("Buttons/Blue").GetComponent<Button>();
    }

    public override void Hide()
    {
        blueBtn.onClick.RemoveAllListeners();
        redBtn.onClick.RemoveAllListeners();
        greenBtn.onClick.RemoveAllListeners();
        
        blueBtn.gameObject.SetActive(false);
        redBtn.gameObject.SetActive(false);
        greenBtn.gameObject.SetActive(false);
        
       
        foreach (ItemBase item in items)
        {
            PoolingManager.Despawn(item.gameObject);
        }
        base.Hide();
    }

    public override void OnFinishedShow()
    {
        
        base.OnFinishedShow();
        ShowReward();
    }

    private async void ShowReward()
    {
        items.Clear();
        items = await InventoryManager.Instance.ShowItemInLoot(rewardHolder);
    }

    public void SetLoseAchiveMent()
    {
        if(greenBtn != null) greenBtn.gameObject.SetActive(false);
        if(blueBtn != null) blueBtn.gameObject.SetActive(false);
        if(redBtn != null) redBtn.gameObject.SetActive(true);
    }

    private void SetButtonInfor(Button btn, string textInfor, Action action = null, int siblingIndex = 0)
    {
        btn.gameObject.SetActive(true);
        
        var textComp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null) textComp.text = textInfor;

        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(() => action?.Invoke());
        btn.transform.SetSiblingIndex(siblingIndex);
    }

    public void SetBlueBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(blueBtn, textInfor, action, siblingIndex);
    public void SetRedBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(redBtn, textInfor, action, siblingIndex);
    public void SetGreenBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(greenBtn, textInfor, action, siblingIndex);
}
