
using System;
using System.Collections.Generic;
using AudioSystem;
using Game.UI;
using TMPro;
using UnityEngine;



public class AchivementUI : UIView
{
   
    [SerializeField] private Transform rewardHolder;
    private List<ItemBase> items = new List<ItemBase>();
    [SerializeField] private ButtonAnimBase redBtn;
    [SerializeField] private ButtonAnimBase greenBtn;
    [SerializeField] private ButtonAnimBase blueBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
        redBtn = transform.Find("Buttons/Red").GetComponent<ButtonAnimBase>();
        greenBtn = transform.Find("Buttons/Green").GetComponent<ButtonAnimBase>();
        blueBtn = transform.Find("Buttons/Blue").GetComponent<ButtonAnimBase>();
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        base.Show();
    }

    public override void Hide()
    {
        blueBtn.onClick = null;
        redBtn.onClick = null;
        greenBtn.onClick = null;
        
        blueBtn.gameObject.SetActive(false);
        redBtn.gameObject.SetActive(false);
        greenBtn.gameObject.SetActive(false);
        
       
        foreach (ItemBase item in items)
        {
            PoolingManager.Despawn(item.gameObject);
        }
        
        base.Hide();
        ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
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



    private void SetButtonInfor(ButtonAnimBase btn, string textInfor, Action action = null, int siblingIndex = 0)
    {
        btn.gameObject.SetActive(true);
        
        var textComp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (textComp != null) textComp.text = textInfor;

        btn.onClick = null;
        btn.onClick += () => action?.Invoke();
        btn.transform.SetSiblingIndex(siblingIndex);
    }

    public void SetBlueBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(blueBtn, textInfor, action, siblingIndex);
    public void SetRedBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(redBtn, textInfor, action, siblingIndex);
    public void SetGreenBtn(string textInfor, Action action = null, int siblingIndex = 0) => SetButtonInfor(greenBtn, textInfor, action, siblingIndex);
}
