
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDataCtrl : ComponentBehaviour
{
    public PlayerCardData PlayerCardData;
    [SerializeField] private Image cardSurface;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardType;
    [SerializeField] private TextMeshProUGUI cardDesciption;
    [SerializeField] private TextMeshProUGUI manaCostTxt;
   
    private CardStrategy cardStrategy;

    public CardStrategy CardStrategy
    {
        get => cardStrategy;
        set => cardStrategy = value;
    }

    private int manaCost;

    public int ManaCost
    {
        get => manaCost;
        set
        {
            manaCost = value;
            manaCostTxt.text = manaCost.ToString();
            if(!_isRewardCard) manaCostTxt.color = ManaCost <= InGameManager.Instance.CurMana ? Color.white : Color.red;
        }
    }

    private bool _isRewardCard;
  
    public bool CanUseData() => PlayerCardData != null && CardStrategy != null;
    public override void LoadComponent()
    {
        base.LoadComponent();
        Transform cardView = transform.parent.Find("Card View");
        if(manaCostTxt == null) manaCostTxt = cardView.Find("Cost").GetComponent<TextMeshProUGUI>();
        if (cardName == null) cardName = cardView.Find("Name").GetComponent<TextMeshProUGUI>();
        if (cardImage == null) cardImage = cardView.Find("Card Image").GetComponent<Image>();
        if (cardType == null) cardType = cardView.Find("Card Type").GetComponent<Image>();
        if (cardDesciption == null) cardDesciption = cardView.Find("Description").GetComponent<TextMeshProUGUI>();
        if (cardSurface == null) cardSurface = cardView.Find("CardSurface")?.GetComponent<Image>();
    }

    private void OnEnable()
    {
        if(cardSurface != null) cardSurface.gameObject.SetActive(false);
        InGameManager.Instance.OnManaChange += OnManaChange;
        if(!_isRewardCard) manaCostTxt.color = ManaCost <= InGameManager.Instance.CurMana ? Color.white : Color.red;
        ObserverManager<CardActionType>.Attach(CardActionType.IncreaseManaCost, OnIncreaseManaCost);
    }

    private void OnIncreaseManaCost(object param)
    {
        ManaCost += (int)param;
    }

    public void Init(PlayerCardData playerCardData, bool isRewardCard)
    {
        PlayerCardData = playerCardData;
        cardStrategy = playerCardData.CardStrategy;
        if (PlayerCardData == null)
        {
            Debug.LogWarning("Player card data is null");
            return;
        }
        ManaCost = PlayerCardData.ManaCost;
     
        manaCostTxt.color = Color.white;
    
        cardImage.sprite = PlayerCardData.CardImage;
        cardDesciption.text = PlayerCardData.CardDescription;
        cardType.sprite = PlayerCardData.CardTypeSprite;
        cardName.text = PlayerCardData.CardName;
        cardName.color = PlayerCardData.CardNameColor;
        _isRewardCard = isRewardCard;

        if (cardSurface != null) cardSurface.gameObject.SetActive(false);
        
    }

    private void OnDisable()
    {
        if (InGameManager.Instance == null) return;
        InGameManager.Instance.OnManaChange -= OnManaChange;
        ObserverManager<CardActionType>.Detach(CardActionType.IncreaseManaCost, OnIncreaseManaCost);
    }

    private void OnManaChange(int value)
    {
        if (ManaCost > value) manaCostTxt.color = Color.red;
        else manaCostTxt.color = Color.white;
    }

    public void SetCardSurface(Sprite surfaceSprite, Material material = null)
    {
        if (cardSurface == null) return;
       
        cardSurface.sprite = surfaceSprite;
        if (material != null) cardSurface.material = material;
        cardSurface.gameObject.SetActive(true);
    }
}
