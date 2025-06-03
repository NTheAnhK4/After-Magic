
using TMPro;
using UnityEngine;

public class CardDataCtrl : ComponentBehavior
{
    public PlayerCardData PlayerCardData;
    public CardStrategy CardStrategy => PlayerCardData.CardStrategy;
   
    
    public int ManaCost { get; private set;}
    private TextMeshPro costText;

  
    public bool CanUseData() => PlayerCardData != null;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        Transform cardView = transform.parent.Find("Card View");
        costText = cardView.Find("Cost").GetComponent<TextMeshPro>();
    }

    private void OnEnable()
    {
        InGameManager.Instance.OnManaChange += OnManaChange;
        if (PlayerCardData != null)
        {
            ManaCost = PlayerCardData.ManaCost;
            if (costText != null)
            {
                costText.text = ManaCost.ToString();
                costText.color = Color.white;
            }
        }
       
    }

    private void OnDisable()
    {
        InGameManager.Instance.OnManaChange -= OnManaChange;
    }

    private void OnManaChange(int value)
    {
        if (ManaCost > value) costText.color = Color.red;
        else costText.color = Color.white;
    }
}
