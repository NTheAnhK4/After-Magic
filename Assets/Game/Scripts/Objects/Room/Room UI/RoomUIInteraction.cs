using Game.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomUIInteraction : RoomUIComponent, IDragHandler
{
   
    private RectTransform rectTransform;
    private Vector2 posXLimit;
    private Vector2 posYLimit;
    private float roomDistance = 150f;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (rectTransform == null) rectTransform = transform.Find("Rooms").GetComponent<RectTransform>();
       
    }

    public void Init()
    {
        float roomHorizontalLength = DungeonMapUI.rooms[0].Count * roomDistance;
        posXLimit = new Vector2(-1 * roomHorizontalLength / 2, roomHorizontalLength / 2);
        float roomVerticalLength = DungeonMapUI.rooms.Count * roomDistance;
        posYLimit = new Vector2(-1 * roomVerticalLength / 2, roomVerticalLength / 2);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var anchoredPosition = rectTransform.anchoredPosition + eventData.delta/ DungeonMapUI.UIScreen.Canvas.scaleFactor;
        float posX = anchoredPosition.x;
        if (posX <= posXLimit.x || posX >= posXLimit.y) return;
        float posY = anchoredPosition.y;
        if (posY <= posYLimit.x || posY >= posYLimit.y) return;
        
        rectTransform.anchoredPosition = anchoredPosition;
    }
}