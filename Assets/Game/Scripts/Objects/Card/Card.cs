

using UnityEngine;
using UnityEngine.Rendering;



public  class Card : ComponentBehaviour
{



    [SerializeField] private CanvasGroup canvasGroup;
    public CardDataCtrl CardDataCtrl { get; private set; }
    public CardAnimation CardAnimation { get; private set; }
    public CardTargetHandler CardTargetHandler { get; private set; }
    public CardAction CardAction { get; private set; }

    
   

    public override void LoadComponent()
    {
        base.LoadComponent();
    
        if (CardDataCtrl == null) CardDataCtrl = GetComponentInChildren<CardDataCtrl>();
        if (CardAnimation == null) CardAnimation = GetComponent<CardAnimation>();
        if (CardTargetHandler == null) CardTargetHandler = GetComponent<CardTargetHandler>();
        if (CardAction == null) CardAction = GetComponent<CardAction>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }

    public void SetUsable(bool canUse, bool isWhiteColor )
    {
        canvasGroup.interactable = canUse;
        canvasGroup.blocksRaycasts = canUse;
        CardAnimation.SetColor(isWhiteColor);
       
    }
}

