

using UnityEngine;
using UnityEngine.Rendering;



public  class Card : ComponentBehavior
{
   
    
   
   
     public CardDataCtrl CardDataCtrl { get; private set; }
    public CardAnimation CardAnimation { get; private set; }
    public CardTargetHandler CardTargetHandler { get; private set; }
    public CardAction CardAction { get; private set; }

    

    public readonly string inHandLayer = "CardInHand";
    public readonly string selectedLayer = "CardSelected";
    
  
   
    public SortingGroup SortingGroup { get; set; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (SortingGroup == null) SortingGroup = GetComponent<SortingGroup>();
        if (CardDataCtrl == null) CardDataCtrl = GetComponentInChildren<CardDataCtrl>();
        if (CardAnimation == null) CardAnimation = GetComponent<CardAnimation>();
        if (CardTargetHandler == null) CardTargetHandler = GetComponent<CardTargetHandler>();
        if (CardAction == null) CardAction = GetComponent<CardAction>();
    }


}

