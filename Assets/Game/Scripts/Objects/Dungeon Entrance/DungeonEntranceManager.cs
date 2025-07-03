
using System.Collections.Generic;
using System.Linq;
using Game.UI;
using SaveGame;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class DungeonEntranceManager : Singleton<DungeonEntranceManager>
{
    [SerializeField] private List<DungeonEntrance> DungeonEntrances = new();
    [SerializeField] private CanvasGroup canvasGroup;
  
    public override void LoadComponent()
    {
        base.LoadComponent();
        DungeonEntrances ??= new List<DungeonEntrance>();
        if (DungeonEntrances.Count == 0) DungeonEntrances = transform.GetComponentsInChildren<DungeonEntrance>().ToList();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    private async void OnDungeonEntranceClick(int id)
    {
        GameManager.Instance.CurrentWorldID = id;
        canvasGroup.interactable = false;
        await UIScreen.Instance.ShowUI<WorldDescriptionUI>();
        canvasGroup.interactable = true;
    }

    private void OnEnable()
    {
        if (DungeonEntrances == null) return;
        for (int i = 0; i < DungeonEntrances.Count; ++i)
        {
            int id = i;
            DungeonEntrances[i].button.onClick.AddListener(() =>OnDungeonEntranceClick(id));
        }
    }

    private void OnDisable()
    {
        if (DungeonEntrances == null) return;
        for (int i = 0; i < DungeonEntrances.Count; ++i)
        {
            DungeonEntrances[i].button.onClick.RemoveAllListeners();
        }
    }

    private void Start()
    {
       LockAllDungeonEntrance();
       if(SaveLoadSystem.Instance.GameData == null) DungeonEntrances[0].UnLockDungeonEntrance();
       else
       {
           for(int i = 0;  i <= SaveLoadSystem.Instance.GameData.WorldUnlockedID; ++i) DungeonEntrances[i].UnLockDungeonEntrance();
       }
    }

    public void LockAllDungeonEntrance()
    {
        foreach (DungeonEntrance de in DungeonEntrances)
        {
            if(de == null) continue;
            de.LockDungeonEntrance();
        }
    }
}
