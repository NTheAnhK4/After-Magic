


using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

using UnityEngine;

using UnityEngine.UI;
using Random = UnityEngine.Random;
using Sequence = DG.Tweening.Sequence;


public  class ItemBase : ComponentBehavior
{
    [SerializeField] protected ItemType itemType;
    
    [SerializeField] private TextMeshProUGUI amountTxt;
    [SerializeField] protected Button rewardBtn;
    [SerializeField] protected Image rewardBackground;
    [SerializeField] protected Image rewardImg;
    private Vector3 rewardImgPosition;
    private float duration = .3f;

    public Vector3 RewardPosition;
    public int amount;

    protected int minAmountGained;
    protected int maxAmountGained;
    protected int bossRewardMultiplier;
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
        
        if(amountTxt != null) amountTxt.text = amount.ToString();
    }

    public void SetInteracable(bool interacable)
    {
        rewardBtn.gameObject.SetActive(interacable);
    }
    

    public virtual async UniTask ShowReward(int amountGained = -1)
    { 
        if(amountGained < 0) amountGained = Random.Range(minAmountGained, maxAmountGained + 1);
        SetAmount(amountGained);
        transform.localScale = Vector3.zero;
       
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(1.2f, duration).SetEase(Ease.OutBack))
            .Append(transform.DOScale(1.0f, duration / 2).SetEase(Ease.OutBack)).SetUpdate(true);

        await seq.AsyncWaitForCompletion();
    }

    protected virtual void OnEnable()
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
        if(CanTakeRewardToLoot()) InventoryManager.Instance.AddToLoot(itemType, amount);
        RewardManager.Instance.RemoveCurrentReward(this);
        PoolingManager.Despawn(gameObject);
    }

    protected virtual bool CanTakeRewardToLoot() => false;


}
