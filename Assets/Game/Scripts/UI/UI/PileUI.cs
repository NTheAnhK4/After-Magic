using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Defines;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PileUI : UIView
{
    private List<Card> cardPile = new List<Card>();

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject cardHolder;
    [SerializeField] private Button exitBtn;
    public void Init(List<Card> cards, string title)
    {
        cardPile = cards;
      
        titleText.text = title;
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (cardHolder == null) cardHolder = transform.Find("Scroll View").Find("Viewport").Find("Content").gameObject;
        if (exitBtn == null) exitBtn = transform.Find("Exit").GetComponent<Button>();
        if (titleText == null) titleText = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        ShowAnimation = ViewAnimationType.DipToBlack;
        HideAnimation = ViewAnimationType.DipToBlack;
    }
    
    private void OnEnable()
    {
        exitBtn.onClick.AddListener(() => UIScreen.HideUI(this));
    }

    private void OnDisable()
    {
        exitBtn.onClick.RemoveAllListeners();
    }

    public override void Show()
    {
        base.Show();
        if(cardPile != null) ShowCards();
    }

    public override void Hide()
    {
        HideCards();
        base.Hide();
        
    }

    private void ShowCards()
    {
        foreach (Card card in cardPile)
        {
            if(card == null) continue;
            card.SetUsable(false, true);
            GameObject cardObj = PoolingManager.Spawn(card.gameObject, cardHolder.transform);
            cardObj.transform.localScale = Vector3.one;
        }
    }

    private void HideCards()
    {
        foreach (Transform card in cardHolder.transform)
        {
            
            PoolingManager.Despawn(card.gameObject);
        }
    }
}
