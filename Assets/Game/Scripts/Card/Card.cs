
using Cysharp.Threading.Tasks;
using DG.Tweening;
using StateMachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;


public  class Card : ComponentBehavior
{
    [FormerlySerializedAs("SkillStrategy")] public CardStrategy cardStrategy;
    
    [HideInInspector] public CardManager CardManager;
    [HideInInspector] public Player Player;
    private Vector3 prePosition;
    private Quaternion preQuaternion;
    private Vector3 preScale;

    public bool CanUseCard { get; set; }

    public readonly string inHandLayer = "CardInHand";
    public readonly string selectedLayer = "CardSelected";
    
  
    private bool hasDragged;
    private Vector3 offset;
    private float zCoord;
    [SerializeField] private SortingGroup sortingGroup;

    private Entity cardTarget;
    
    
    #region Unity Functions
    
    private void OnEnable()
    {
        SetSortingLayer(inHandLayer);
        cardTarget = null;  
    }
    
    
    private void OnMouseDown()
    {
        if (!CanUseCard) return;
        if(cardStrategy == null) return;
        ObserverManager<CardTargetType>.Notify(cardStrategy.AppliesToAlly ? CardTargetType.Player :CardTargetType.Enemy, true); 
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        
        Vector3 mousePoint = GetMouseWorldPosition();
        offset = transform.position - mousePoint;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.localScale = new Vector3(.8f, .8f, 1);
        GameManager.Instance.SetTurn(GameStateType.UsingCard);
        CanUseCard = true;
    }
    

    private void OnMouseDrag()
    {
        if (!CanUseCard) return;
        SetSortingLayer(selectedLayer);
        hasDragged = true;
        Vector3 mousePoint = GetMouseWorldPosition();
        
        transform.position = mousePoint + offset;
    }

    private void OnMouseUp()
    {
        ObserverManager<CardTargetType>.Notify(cardStrategy.AppliesToAlly ? CardTargetType.Player :CardTargetType.Enemy, false); 
        if (!CanUseCard) return;
       
        if (cardTarget != null && cardStrategy != null && CardManager != null)
        {
            UseCard();
            return;
        }
        if (hasDragged)
        {
            ReturnHand();
            hasDragged = false;
        }
    }

   

    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity target = other.transform.GetComponent<Entity>();

        if(!IsCardTarget(target)) return;
        cardTarget = target;
        cardTarget.SelectObject();
    }
    

    private void OnTriggerExit2D(Collider2D other)
    {
        Entity target = other.transform.GetComponent<Entity>();
        if(!IsCardTarget(target)) return;
       
        target.DeselectObject();
        cardTarget = null;
        
    }
    

    #endregion
    
    
   
   
    #region Sorting Group

    public void SetSortingOrder(int sortingValue)
    {
        if(sortingGroup != null) sortingGroup.sortingOrder = sortingValue;
        
        SetPreTransformValue();
    }

    public void SetPreTransformValue()
    {
        var transform1 = transform;
        prePosition = transform1.position;
        preQuaternion = transform1.rotation;
        preScale = transform1.localScale;
    }
    public void SetSortingLayer(string layerName)
    {
        if(sortingGroup != null) sortingGroup.sortingLayerName = layerName;
    }

    #endregion

    #region Other Functions

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (sortingGroup == null) sortingGroup = GetComponent<SortingGroup>();
        
    }

    private bool IsCardTarget(Entity target)
    {
        if (target == null || cardStrategy == null) return false;
        if (cardStrategy.AppliesToAlly) return target.transform.tag.Equals("Player");
        return target.transform.tag.Equals("Enemy");
    }
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

  

    private async void UseCard()
    {
        if (cardTarget != null)
        {
            cardTarget.DeselectObject();
            if (!cardStrategy.AppliesToAlly) CardManager.Player.EnemyTarget = cardTarget.transform.GetComponent<Entity>();
        }
       
        
        CardManager.Player.CardStrategy = cardStrategy;
        CardManager.Player.MustReachTarget = cardStrategy.MustReachTarget;
        await CardManager.CollectingCard(this, false);
    }

    private async void ReturnHand()
    {
        SetSortingLayer(inHandLayer);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOMove(prePosition,.3f))
            .Join(transform.DORotate(preQuaternion.eulerAngles,.3f))
            .Join(transform.DOScale(preScale, .3f));
        await UniTask.WaitUntil(() => !sequence.IsActive());
        GameManager.Instance.SetTurn(GameStateType.PlayerTurn);
    }
    
    
    
    #endregion
}

