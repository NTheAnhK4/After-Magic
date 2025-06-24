using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonEntrance : ComponentBehavior, IPointerDownHandler,IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CanvasGroup canvasGroup;
    
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void LockDungeonEntrance()
    {
        canvasGroup.alpha = .25f;

        canvasGroup.blocksRaycasts = false;
    }

    public void UnLockDungeonEntrance()
    {
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        transform.DOKill();
        
        transform.DOScale(1.2f, .45f).SetUpdate(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOKill();
        transform.DOScale(1, .45f).SetUpdate(true);
       
    }
    
    private Vector2 start;
    private float startTime;
    private const float dragThreshold = 10f;      
    private const float timeThreshold = 0.3f;     

    public void OnPointerDown(PointerEventData eventData)
    {
        start = eventData.position;
        startTime = Time.unscaledTime;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float distance = Vector2.Distance(eventData.position, start);
        float timeHeld = Time.unscaledTime - startTime;

        if (distance < dragThreshold && timeHeld < timeThreshold) EnterMap();
    }

    private async void EnterMap()
    {
        DungeonEntranceManager.Instance.LockAllDungeonEntrance();
        Transform transform1;
        (transform1 = transform).DOKill(); 
        transform1.localScale = Vector3.one;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0.85f, 0.1f).SetEase(Ease.OutQuad));
        seq.Append(transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        seq.SetUpdate(true);
        await seq.AsyncWaitForCompletion();
        SceneLoader.Instance.LoadScene(GameConstants.DungeonScene);
    }
    
}
