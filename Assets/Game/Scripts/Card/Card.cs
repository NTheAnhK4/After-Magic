
using System;
using UnityEngine;
using UnityEngine.Rendering;

public class Card : ComponentBehavior
{
    private Vector3 prePosition;
    private Quaternion preQuaternion;
    private bool isSelectCard;

    public readonly string inHandLayer = "CardInHand";
    public readonly string selectedLayer = "CardSelected";
    
    [SerializeField] private SortingGroup sortingGroup;
   
    

    #region Unity Functions
    
    private void OnEnable()
    {
        ObserverManager<CardEventID>.Attach(CardEventID.DeselectAllCard, param => DeSelectCard());
        
        isSelectCard = false;
        SetSortingLayer(inHandLayer);
    }

    private void OnDisable()
    {
        ObserverManager<CardEventID>.DetachAll();
    }

    private void OnMouseDown()
    {
        isSelectCard = !isSelectCard;
        if (isSelectCard)
        {
            ObserverManager<CardEventID>.Notify(CardEventID.DeselectAllCard);
            SelectCard();
        }
        else DeSelectCard();
    }

    #endregion
    

    #region Handle Select Card

    private void SelectCard()
    {
        SetSortingLayer(selectedLayer);
        // Select this card after deselecting all others
        isSelectCard = true;
        
        var transform1 = transform;
        transform1.position = new Vector3(prePosition.x, prePosition.y + 2.5f, 0);
        transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
    }

    private void DeSelectCard()
    {
        SetSortingLayer(inHandLayer);
        // Deselect all cards when the DeselectAllCard event is posted
        isSelectCard = false;
        
        var transform1 = transform;
        transform1.position = prePosition;
        transform1.rotation = preQuaternion;
    }


    #endregion
   
    #region Sorting Group

    public void SetSortingOrder(int sortingValue)
    {
        sortingGroup.sortingOrder = sortingValue;
        var transform1 = transform;
        prePosition = transform1.position;
        preQuaternion = transform1.rotation;

    }
    
    public void SetSortingLayer(string layerName)
    {
        sortingGroup.sortingLayerName = layerName;
    }

    #endregion

    #region Other Functions

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (sortingGroup == null) sortingGroup = GetComponent<SortingGroup>();
        
    }

    #endregion
}
