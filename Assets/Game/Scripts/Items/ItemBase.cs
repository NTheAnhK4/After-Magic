


using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;


public  class ItemBase : ComponentBehavior
{
    public ItemData ItemData;
    [SerializeField] private TextMeshProUGUI amountTxt;
    [SerializeField] protected Button rewardBtn;
    [SerializeField] protected Image rewardBackground;
    [SerializeField] protected Image rewardImg;
    private Vector3 rewardImgPosition;
    private float duration = .3f;

    public Vector3 RewardPosition;
    public int amount;
    
   
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (amountTxt == null) amountTxt = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        if (rewardBtn == null) rewardBtn = transform.Find("Background").GetComponent<Button>();
        if (rewardImg == null) rewardImg = transform.Find("RewardImg").GetComponent<Image>();
        if (rewardBackground == null) rewardBackground = transform.Find("Background").GetComponent<Image>();
    }

    protected override void Awake()
    {
        base.Awake();
        rewardImgPosition = transform.position;
    }

    public void SetAmount(int amountValue)
    {
        amount = amountValue;
        if(ItemData != null) ItemData.Amount = amountValue;
    }

    public void SetInteracable(bool interacable)
    {
        rewardBtn.gameObject.SetActive(interacable);
    }
    

    public virtual async UniTask ShowReward()
    {
        transform.localScale = Vector3.zero;
        amountTxt.text = amount.ToString();
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.2f, duration).SetEase(Ease.OutBack))
            .Append(transform.DOScale(1.0f, duration / 2).SetEase(Ease.OutBack)).SetUpdate(true);

        await seq.AsyncWaitForCompletion();
    }

    private void OnEnable()
    {
        rewardBtn.onClick.AddListener(()  => GainReward().Forget());
        rewardImg.transform.position = rewardImgPosition;

        Color color = rewardBackground.color;
        color.a = 1;
        rewardBackground.color = color;
        

    }

    private void OnDisable()
    {
        rewardBtn.onClick.RemoveAllListeners();
    }

    public virtual async UniTask GainReward()
    {
        await GainRewardAnim();

        Despawn();
    }

    protected virtual async UniTask GainRewardAnim()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(rewardImg.transform.DOMove(RewardPosition,.3f))
            .Join(rewardBackground.DOFade(0,.3f)).SetUpdate(true);

        await seq.AsyncWaitForCompletion();
    }

    protected virtual void Despawn()
    {
        RewardManager.Instance.TakeEachReward();
        if(ItemData != null) InventoryManager.Instance.AddToLoot(ItemData);
        RewardManager.Instance.RemoveCurrentReward(this);
        PoolingManager.Despawn(gameObject);
    }
   
    
}
