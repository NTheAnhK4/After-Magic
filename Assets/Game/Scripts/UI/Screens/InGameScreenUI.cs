
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    [SerializeField] private Button mapBtn;

    [SerializeField] private TextMeshProUGUI drawPileTxt;
    [SerializeField] private TextMeshProUGUI discardPileTxt;
    [SerializeField] private TextMeshProUGUI depleteCardsTxt;
    [Header("Mana")] [SerializeField] private TextMeshProUGUI manaTxt;



    [SerializeField] private TextMeshProUGUI coinTxt;
  
    public Action OnRevivePlayer;
  

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
        FindUI(ref mapBtn, "Top/Top Right/Map");

        FindUI(ref drawPileTxt, cardDesk, "Draw Pile/Text (TMP)");
        FindUI(ref discardPileTxt, cardDesk, "Discard Pile/Text (TMP)");
        FindUI(ref depleteCardsTxt, cardDesk, "Deplete Cards/Text (TMP)");
        
        FindUI(ref coinTxt, "Top/Top Left/Coin Infor/CoinTxt");
        
        AddUIView<WinUI>();
        AddUIView<LoseUI>();
        
        AddUIView<WinUI>();
        AddUIView<LoseUI>();
        AddUIView<AchivementUI>();
        AddUIView<PauseUI>();
        AddUIView<SettingUI>();
        AddUIView<PileUI>();
        AddUIView<DungeonMapUI>();
        AddUIView<CardRewardUI>();

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

   
   

    private async void Start()
    {
        
        MusicManager.Instance.PlayMusic(MusicType.Dungeon);
        OnRevivePlayer += RevivePlayer;

        DungeonMapUI dungeonMapUI = GetUIView<DungeonMapUI>();
        dungeonMapUI.IsVirtualMap = false;
        await ShowUI<DungeonMapUI>();

    }

  
    private void OnEnable()
    {
        if (coinTxt != null) coinTxt.text = GameManager.Instance.CoinAmount.ToString();
        onUsingCardAction = param => turnBtn.interactable = false;
        onWinAction = param =>  ShowUI<WinUI>().Forget();
        onLoseAction = param => ShowUI<LoseUI>().Forget();
        
      
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
        
        ObserverManager<GameEventType>.Attach(GameEventType.GainCoin, GainCoin);
    }

    private void UnregisterEvents()
    {
        if (InGameManager.Instance != null) InGameManager.Instance.OnManaChange -= OnManaChange;
       
        ObserverManager<GameStateType>.Detach(GameStateType.PlayerTurn, OnPlayerTurn);
        ObserverManager<GameStateType>.Detach(GameStateType.UsingCard, onUsingCardAction);
        ObserverManager<GameEventType>.Detach(GameEventType.Win, onWinAction);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose, onLoseAction);
        
        ObserverManager<CardEventType>.Detach(CardEventType.DrawPileCountChange, OnDrawPileCountChange);
        ObserverManager<CardEventType>.Detach(CardEventType.DiscardPileCountChange, OnDiscardPileCountChange);
        ObserverManager<CardEventType>.Detach(CardEventType.DepleteCardsCountChange, OnDepleteCardsCountChange);

        ObserverManager<GameEventType>.Detach(GameEventType.GainCoin, GainCoin);
    }

    private void RegisterUIEvents()
    {
        mapBtn.onClick.AddListener(ShowMap);
        turnBtn.onClick.AddListener(OnTurnBtnClick);
        pauseBtn.onClick.AddListener(OnPauseBtnClick);
        turnBtn.interactable = false;
        drawPileBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DrawPile, "Draw Pile"));
        discardPileBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DisCardPile, "Discard Pile"));
        depleteCardsBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.DepleteCards, "Deplete Cards"));
        availableCardsBtn.onClick.AddListener(() => ShowCardPile(CardManager.Instance.MainDesk, "Main Desk"));
    }

    private async void OnPauseBtnClick() => await ShowUI<PauseUI>();
    private void UnregisterUIEvents()
    {
        mapBtn.onClick.RemoveAllListeners();
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

   
  
 
    private async void RevivePlayer()
    {
        await HideUIOnTop();
        InGameManager.Instance.RevivePlayer();
    }

    private async void ShowCardPile(List<PlayerCardData> cards, string title)
    {
        PileUI pileUI = GetUIView<PileUI>();
        pileUI.Init(cards,title);
        await ShowUI<PileUI>();
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

    private async void ShowMap()
    {
        GetUIView<DungeonMapUI>().IsVirtualMap = true;
        await ShowUI<DungeonMapUI>();
        
    }

    private void GainCoin(object amount)
    {
        if (coinTxt == null)
        {
            Debug.LogWarning("Coin text is null");
            return;
        }
        int coinAdd = (int)amount;
        int currentCoin = Convert.ToInt32(coinTxt.text);
        coinTxt.text = (coinAdd + currentCoin).ToString();
    }

   
}
