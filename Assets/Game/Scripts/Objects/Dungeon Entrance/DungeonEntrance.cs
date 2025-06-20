using System.Collections;
using System.Collections.Generic;
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
        transform.localScale = Vector3.one *1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
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

        if (distance < dragThreshold && timeHeld < timeThreshold)
        {
            SceneLoader.Instance.LoadScene(GameConstants.DungeonScene);
           
        }
        
    }
}
