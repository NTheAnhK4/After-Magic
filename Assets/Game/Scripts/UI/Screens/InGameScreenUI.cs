
using System;
using System.Collections.Generic;
using Game.UI;
using TMPro;
using UnityEngine;

using UnityEngine.UI;


public class InGameScreenUI : UIScreen
{
    [Header("Button")]
    [SerializeField] private Button turnBtn;

    [SerializeField] private TextMeshProUGUI turnTxt;
    [SerializeField] private Button pauseBtn;
    [SerializeField] private Button drawPileBtn;
    [SerializeField] private Button discardPileBtn;
    [SerializeField] private Button depleteCardsBtn;

    [SerializeField] private Button availableCardsBtn;

    [SerializeField] private TextMeshProUGUI drawPileTxt;
    [SerializeField] private TextMeshProUGUI discardPileTxt;
    [SerializeField] private TextMeshProUGUI depleteCardsTxt;
    [Header("Mana")] [SerializeField] private TextMeshProUGUI manaTxt;

   
    [Header("UI View")] 
    [SerializeField] private WinUI winUI;

    [SerializeField] private LoseUI loseUI;
    [SerializeField] private AchivementUI achivementUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private SettingUI settingUI;
    [SerializeField] private PileUI pileUI;

    public Action OnShowAchivement;
    public Action OnRevivePlayer;
    public Action OnShowSetting;

    private Action<object> onUsingCardAction;
    private Action<object> onWinAction;
    private Action<object> onLoseAction;

    
    
    public override void LoadComponent()
    {
        base.LoadComponent();
        FindUI(ref turnBtn, "Turn Button");
        FindUI(ref turnTxt, "Turn Button/Text (TMP)");
        FindUI(ref manaTxt, "Mana/Text (TMP)");
        FindUI(ref pauseBtn, "Top/Top Right/Pause");
        FindUI(ref availableCardsBtn, "Top/Top Right/Card Desk");
    
        Transform cardDesk = transform.Find("Card Desk");
        FindUI(ref drawPileBtn, cardDesk, "Draw Pile");
        FindUI(ref discardPileBtn, cardDesk, "Discard Pile");
        FindUI(ref depleteCardsBtn, cardDesk, "Deplete Cards");

        FindUI(ref drawPileTxt, cardDesk, "Draw Pile/Text (TMP)");
        FindUI(ref discardPileTxt, cardDesk, "Discard Pile/Text (TMP)");
        FindUI(ref depleteCardsTxt, cardDesk, "Deplete Cards/Text (TMP)");

        Transform ui = transform.parent.Find("UI");
        InitUI(ref winUI, ui);
        InitUI(ref loseUI, ui);
        InitUI(ref achivementUI, ui);
        InitUI(ref pauseUI, ui);
        InitUI(ref settingUI, ui);
        InitUI(ref pileUI, ui);

        if (turnTxt != null) turnTxt.text = "End Turn";
    }

    private void FindUI<T>(ref T target, string path) where T : Component
    {
        if (target == null) target = transform.Find(path)?.GetComponent<T>();
    }
    private void FindUI<T>(ref T target, Transform root, string path) where T : Component
    {
        if (target == null) target = root.Find(path)?.GetComponent<T>();
    }

   
   

    private void Start()
    {
        OnShowAchivement += () => ShowAfterHide(achivementUI);
        OnShowSetting += () => ShowAfterHide(settingUI);
        OnRevivePlayer += RevivePlayer;
        
       
    }

    private void OnEnable()
    {
        onUsingCardAction = param => turnBtn.interactable = false;
        onWinAction = param => ShowUI(winUI);
        onLoseAction = param => ShowUI(loseUI);
        
      
        RegisterUIEvents();
        RegisterEvents();
       
        
    }
    
    private void OnDisable()
    {
        UnregisterUIEvents();
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InGameManager.Instance.OnManaChange += OnManaChange;
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, OnPlayerTurn);
        ObserverManager<GameStateType>.Attach(GameStateType.UsingCard, onUsingCardAction);
        ObserverManager<GameEventType>.Attach(GameEventType.Win, onWinAction);
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, onLoseAction);
        
        
        ObserverManager<CardEventType>.Attach(CardEventType.DrawPileCountChange, OnDrawPileCountChange);
        ObserverManager<CardEventType>.Attach(CardEventType.DiscardPileCountChange, OnDiscardPileCountChange);
        ObserverManager<CardEventType>.Attach(CardEventType.DepleteCardsCountChange, OnDepleteCardsCountChange);
    }

    private void UnregisterEvents()
    {
        InGameManager.Instance.OnManaChange -= OnManaChange;
        ObserverManager<GameStateType>.Detach(GameStateType.PlayerTurn, OnPlayerTurn);
        ObserverManager<GameStateType>.Detach(GameStateType.UsingCard, onUsingCardAction);
        ObserverManager<GameEventType>.Detach(GameEventType.Win, onWinAction);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose, onLoseAction);
        
        ObserverManager<CardEventType>.Detach(CardEventType.DrawPileCountChange, OnDrawPileCountChange);
        ObserverManager<CardEventType>.Detach(CardEventType.DiscardPileCountChange, OnDiscardPileCountChange);
        ObserverManager<CardEventType>.Detach(CardEventType.DepleteCardsCountChange, OnDepleteCardsCountChange);

    }

    private void RegisterUIEvents()
    {
        turnBtn.onClick.AddListener(OnTurnBtnClick);
        pauseBtn.onClick.AddListener(() => ShowUI(pauseUI));
        turnBtn.interactable = false;
        drawPileBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DrawPile, "Draw Pile"));
        discardPileBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DisCardPile, "Discard Pile"));
        depleteCardsBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DepleteCards, "Deplete Cards"));
        availableCardsBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.MainDesk, "Main Desk"));
    }
    
    private void UnregisterUIEvents()
    {
        turnBtn.onClick.RemoveAllListeners();
        pauseBtn.onClick.RemoveAllListeners();
        drawPileBtn.onClick.RemoveAllListeners();
        discardPileBtn.onClick.RemoveAllListeners();
        depleteCardsBtn.onClick.RemoveAllListeners();
        availableCardsBtn.onClick.RemoveAllListeners();
    }
    private void OnTurnBtnClick()
    {
        turnTxt.text = "Enemy Turn";
        turnBtn.interactable = false;
        
        InGameManager.Instance.SetTurn(GameStateType.CollectingCard);
    }

    private void OnPlayerTurn(object param)
    {
        turnTxt.text = "End Turn";
        turnBtn.interactable = true;
    }

    private void OnManaChange(int value)
    {
        manaTxt.text = value.ToString() + "/" + InGameManager.Instance.TotalMana.ToString();
    }

   
    private void ShowAfterHide(UIView ui)
    {
        HideUI(null, true);
        ShowUI(ui);
    }

 
    private void RevivePlayer()
    {
        HideUI();
        InGameManager.Instance.RevivePlayer();
    }

    private void ShowCardPile(List<Card> cards, string title)
    {
        HideUI();
        pileUI.Init(cards, title);
        ShowUI(pileUI);
    }

    private void OnDrawPileCountChange(object param)
    {
        drawPileTxt.text = param.ToString();
        
    }
    private void OnDepleteCardsCountChange(object param)
    {
        if((int)param == 0) depleteCardsBtn.gameObject.SetActive(false);
        else depleteCardsBtn.gameObject.SetActive(true);
        depleteCardsTxt.text = param.ToString();
    }
    private void OnDiscardPileCountChange(object param)
    {
        int count = (int)param;
        discardPileBtn.interactable = count > 0;
        discardPileTxt.gameObject.SetActive(count > 0);
        discardPileTxt.text = count.ToString();
    }
}
