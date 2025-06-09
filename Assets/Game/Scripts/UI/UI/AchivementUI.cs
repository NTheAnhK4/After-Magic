
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UI;


public class AchivementUI : UIView
{
   
    [SerializeField] private Transform rewardHolder;
    private List<ItemBase> items = new List<ItemBase>();
    [SerializeField] private Button confirmBtn;
    [SerializeField] private Button stayBtn;
    [SerializeField] private Button goDeepBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rewardHolder == null) rewardHolder = transform.Find("Reward/View/Content");
        confirmBtn = transform.Find("Buttons/Confirm").GetComponent<Button>();
        stayBtn = transform.Find("Buttons/Stay").GetComponent<Button>();
        goDeepBtn = transform.Find("Buttons/Go Deep").GetComponent<Button>();
    }

    public override void OnFinishedShow()
    {
        base.OnFinishedShow();
        ShowReward();
    }

    private async void ShowReward()
    {
        items.Clear();
        foreach (var (key, value) in InventoryManager.Instance.DungeonLoot)
        {
            ItemBase itemBase = PoolingManager.Spawn(key.ItemPrefab.gameObject, rewardHolder).GetComponent<ItemBase>();
            itemBase.SetInteracable(false);
            itemBase.SetAmount(value);
            items.Add(itemBase);
            await itemBase.ShowReward();
        }
    }

    public void SetLoseAchiveMent()
    {
        if(stayBtn != null) stayBtn.gameObject.SetActive(false);
        if(goDeepBtn != null) goDeepBtn.gameObject.SetActive(false);
        if(confirmBtn != null) confirmBtn.gameObject.SetActive(true);
    }
}
