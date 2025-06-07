
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CardDataCtrl : ComponentBehavior
{
    public PlayerCardData PlayerCardData;
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private Image cardImage;
    [SerializeField] private Image cardType;
    [SerializeField] private TextMeshProUGUI cardDesciption;
    [SerializeField] private TextMeshProUGUI manaCost;
    public CardStrategy CardStrategy => PlayerCardData.CardStrategy;
   
    
    public int ManaCost { get; private set;}
    

  
    public bool CanUseData() => PlayerCardData != null;
    public override void LoadComponent()
    {
        base.LoadComponent();
        Transform cardView = transform.parent.Find("Card View");
        if(manaCost == null) manaCost = cardView.Find("Cost").GetComponent<TextMeshProUGUI>();
        if (cardName == null) cardName = cardView.Find("Name").GetComponent<TextMeshProUGUI>();
        if (cardImage == null) cardImage = cardView.Find("Card Image").GetComponent<Image>();
        if (cardType == null) cardType = cardView.Find("Card Type").GetComponent<Image>();
        if (cardDesciption == null) cardDesciption = cardView.Find("Description").GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        InGameManager.Instance.OnManaChange += OnManaChange;

        manaCost.color = ManaCost <= InGameManager.Instance.CurMana ? Color.white : Color.red;
    }

    public void Init(PlayerCardData playerCardData)
    {
        PlayerCardData = playerCardData;
        if (PlayerCardData == null)
        {
            Debug.LogWarning("Player card data is null");
            return;
        }
        ManaCost = PlayerCardData.ManaCost;
        manaCost.text = ManaCost.ToString();
        manaCost.color = Color.white;
    
        cardImage.sprite = PlayerCardData.CardImage;
        cardDesciption.text = PlayerCardData.CardDescription;
        cardType.sprite = PlayerCardData.CardType;
        cardName.text = PlayerCardData.CardName;
    }

    private void OnDisable()
    {
        InGameManager.Instance.OnManaChange -= OnManaChange;
    }

    private void OnManaChange(int value)
    {
        if (ManaCost > value) manaCost.color = Color.red;
        else manaCost.color = Color.white;
    }
}
