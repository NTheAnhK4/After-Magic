
using System.Collections.Generic;
using AudioSystem;
using Game.Defines;
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PileUI : UIView
{
    private List<PlayerCardData> cardPile = new List<PlayerCardData>();
    public GameObject CardShowPrefab;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private GameObject cardHolder;
    [SerializeField] private Button exitBtn;

    [SerializeField] private List<GameObject> cardShowLists = new List<GameObject>();
    public void Init(List<PlayerCardData> cards, string title)
    {
        
        cardPile = new List<PlayerCardData>(cards);
      
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
        exitBtn.onClick.AddListener(OnExitBtnClick );
    }

    private async void OnExitBtnClick()
    {
        await UIScreen.HideUI<PileUI>();
    }

    private void OnDisable()
    {
        exitBtn.onClick.RemoveAllListeners();
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        base.Show();
        if(cardPile != null) ShowCards();
    }

    public override void Hide()
    {
        HideCards();
        base.Hide();
        ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
        
    }

    private void ShowCards()
    {
        cardShowLists.Clear();
        foreach (PlayerCardData playerCardData in cardPile)
        {
            if (playerCardData == null)
            {
                Debug.Log("Player card data is null");
                continue;
            }
            
            
            GameObject cardObj = PoolingManager.Spawn(CardShowPrefab, cardHolder.transform);
            if (cardObj == null)
            {
                Debug.LogWarning("Can not spawn Card Show in pile ui");
                continue;
            }
            cardShowLists.Add(cardObj);
            CardDataCtrl cardDataCtrl = cardObj.GetComponentInChildren<CardDataCtrl>();
            if(cardDataCtrl != null) cardDataCtrl.Init(playerCardData, true);
            cardObj.transform.localScale = Vector3.one;
        }

        
    }

    private void HideCards()
    {
        foreach (GameObject card in cardShowLists)
        {
            PoolingManager.Despawn(card);
        }
     
    }
}
