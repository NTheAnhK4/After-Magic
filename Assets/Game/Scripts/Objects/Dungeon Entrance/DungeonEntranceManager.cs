
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Game.UI;
using SaveGame;

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class DungeonEntranceManager : Singleton<DungeonEntranceManager>
{
    public DungeonEntrance DungeonEntrancePrefab;
    [SerializeField] private ScrollRect scrollRect;
    private List<DungeonEntrance> DungeonEntrances = new();
    [SerializeField] private CanvasGroup canvasGroup;
  
    public override void LoadComponent()
    {
        base.LoadComponent();
        DungeonEntrances ??= new List<DungeonEntrance>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (scrollRect == null) scrollRect = transform.parent.parent.GetComponent<ScrollRect>();
    }

    private async void OnDungeonEntranceClick(int id)
    {
        GameManager.Instance.CurrentWorldID = id;
        canvasGroup.interactable = false;
        await UIScreen.Instance.ShowUI<WorldDescriptionUI>();
        canvasGroup.interactable = true;
    }

    private async void OnEnable()
    {

        await SpawnDungeonEntrance();
        MoveToCurrentWorld();
        
        for (int i = 0; i < DungeonEntrances.Count; ++i)
        {
            int id = i;
            DungeonEntrances[i].button.onClick.AddListener(() =>OnDungeonEntranceClick(id));
        }
        
    }

    private async UniTask SpawnDungeonEntrance()
    {
        DungeonEntrances = new List<DungeonEntrance>();
        for (int i = 0; i < GameManager.Instance.WorldDataCount; ++i)
        {
            WorldData worldData = GameManager.Instance.GetWorldDataById(i);
            if(worldData == null) continue;
            DungeonEntrance dungeonEntrance = PoolingManager.Spawn(DungeonEntrancePrefab.gameObject, transform).GetComponent<DungeonEntrance>();
            if (dungeonEntrance != null)
            {
                dungeonEntrance.GetComponent<Image>().sprite = worldData.WorldSprite;
                dungeonEntrance.transform.localScale = Vector3.one;
                DungeonEntrances.Add(dungeonEntrance);
               
            }
        }

        await UniTask.Delay(200, DelayType.UnscaledDeltaTime);
    }
   



    private void  MoveToCurrentWorld()
    {
        if(scrollRect == null) return;
        
        RectTransform content = GetComponent<RectTransform>();
        
       


        RectTransform target = DungeonEntrances[GameManager.Instance.CurrentWorldID].OrNull()?.GetComponent<RectTransform>();
        if (target == null) return;

        Vector3[] itemWorldCorners = new Vector3[4];
        target.GetWorldCorners(itemWorldCorners);
        float itemWorldCenter = (itemWorldCorners[0].x + itemWorldCorners[3].x) / 2f;

        
        Vector3[] contentWorldCorners = new Vector3[4];
        content.GetWorldCorners(contentWorldCorners);
        float contentLeft = contentWorldCorners[0].x;

       
        float itemCenterLocalX = itemWorldCenter - contentLeft;

        
        float contentWidth = content.rect.width;
        float viewportWidth = scrollRect.viewport.rect.width;
        
        float normalizedPos = (itemCenterLocalX - viewportWidth / 2f) / (contentWidth - viewportWidth);
        normalizedPos = Mathf.Clamp01(normalizedPos);

        scrollRect.horizontalNormalizedPosition = normalizedPos;
    }

    private void OnDisable()
    {
        if (DungeonEntrances == null) return;
        for (int i = 0; i < DungeonEntrances.Count; ++i)
        {
            DungeonEntrances[i].button.onClick.RemoveAllListeners();
        }

        for (int i = DungeonEntrances.Count - 1; i >= 0; --i)
        {
            if(DungeonEntrances[i] != null) PoolingManager.Despawn(DungeonEntrances[i].gameObject);
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
