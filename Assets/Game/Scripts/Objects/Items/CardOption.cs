using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardOption : ComponentBehavior, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    [HideInInspector] public CardRewardUI CardRewardUI;
    [SerializeField] private CardDataCtrl cardDataCtrl;
    private CanvasGroup canvasGroup;
    public override void LoadComponent()
    {
        base.LoadComponent();
        canvasGroup = GetComponent<CanvasGroup>();
        if (cardDataCtrl == null) cardDataCtrl = transform.GetComponentInChildren<CardDataCtrl>();
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.one;
    }

    public void SetCardData(PlayerCardData playerCardData)
    {
        if (cardDataCtrl == null)
        {
            Debug.LogWarning("Card data ctrl is null for " + transform.name);
            return;
        }
        cardDataCtrl.Init(playerCardData, true);
       
    }
    public void SetInteracble(bool interacable){
        if (canvasGroup == null)
        {
            Debug.LogWarning("Canvas Group for " + transform.name + " is null");
            return;
        }

        canvasGroup.interactable = interacable;
        canvasGroup.blocksRaycasts = interacable;
    }

    public async UniTask OnShowCard()
    {
        
        Sequence seq = DOTween.Sequence();
        transform.localScale = Vector3.zero;
        canvasGroup.alpha = 1;
        seq.Append(transform.DOScale(1.2f, .3f).SetEase(Ease.OutBack))
            .Append(transform.DOScale(1f, .15f).SetEase(Ease.OutBack)).SetUpdate(true);
        await seq.AsyncWaitForCompletion();
       
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        CardRewardUI.SetCardInteracble(false);
        Sequence seq = DOTween.Sequence();
        transform.localScale = Vector3.one;
        seq.Append(transform.DOScale(1.1f, .15f).SetEase(Ease.OutBack));
        
        seq.Append(transform.DOShakePosition(0.2f, strength: new Vector3(5f, 5f, 0f), vibrato: 10, randomness: 90, fadeOut: true))
            .Append(transform.DOScale(1, 0.1f)); 

        seq.SetUpdate(true);
        CardRewardUI.DisableOtherCard(this);
        CardManager.Instance.AddCardToMainDesk(cardDataCtrl.PlayerCardData);
       
        PoolingManager.Despawn(gameObject);
    }

   

    public async UniTask DisableCard()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(DOVirtual.Float(1, 0, .3f, a => canvasGroup.alpha = a))
            .Join(transform.DOScale(0, .45f))
            .SetUpdate(true);
        await seq.AsyncWaitForCompletion();
        PoolingManager.Despawn(gameObject);
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.2f, .1f).SetUpdate(true);
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(1f, .1f).SetUpdate(true);
    }
}
