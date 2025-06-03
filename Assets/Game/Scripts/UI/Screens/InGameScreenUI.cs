
using System;

using Game.UI;
using TMPro;
using UnityEngine;

using UnityEngine.UI;


public class InGameScreenUI : UIScreen
{
    [Header("Turn Button")]
    [SerializeField] private Button turnBtn;

    [SerializeField] private TextMeshProUGUI turnTxt;
    [Header("Mana")] [SerializeField] private TextMeshProUGUI manaTxt;

    [SerializeField] private Button pauseBtn;
    [Header("UI View")] 
    [SerializeField] private WinUI winUI;

    [SerializeField] private LoseUI loseUI;
    [SerializeField] private AchivementUI achivementUI;
    [SerializeField] private PauseUI pauseUI;
    [SerializeField] private SettingUI settingUI;

    public Action OnShowAchivement;
    public Action OnRevivePlayer;
    public Action OnShowSetting;
    
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (turnBtn == null) turnBtn = transform.Find("Turn Button").GetComponent<Button>();
        if (turnTxt == null)
        {
            turnTxt = transform.Find("Turn Button").GetComponentInChildren<TextMeshProUGUI>();
            turnTxt.text = "End Turn";
        }

        if (manaTxt == null) manaTxt = transform.Find("Mana").GetComponentInChildren<TextMeshProUGUI>();
        if (pauseBtn == null) pauseBtn = transform.Find("Top").Find("Top Right").Find("Pause").GetComponent<Button>();
        Transform ui = transform.parent.Find("UI");
        InitUI(ref winUI, ui);
        InitUI(ref loseUI, ui);
        InitUI(ref achivementUI, ui);
        InitUI(ref pauseUI, ui);
        InitUI(ref settingUI, ui);
    }

   

    private void Start()
    {
        OnShowAchivement += () => ShowAfterHide(achivementUI);
        OnShowSetting += () => ShowAfterHide(settingUI);
        OnRevivePlayer += RevivePlayer;
    }

    private void OnEnable()
    {
        InGameManager.Instance.OnManaChange += OnManaChange;
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, param => OnPlayerTurn());
        ObserverManager<GameStateType>.Attach(GameStateType.UsingCard, param => turnBtn.interactable = false);
        ObserverManager<GameEventType>.Attach(GameEventType.Win, param => ShowUI(winUI));
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, param => ShowUI(loseUI));
        turnBtn.onClick.AddListener(OnTurnBtnClick);
        pauseBtn.onClick.AddListener(() => ShowUI(pauseUI));
        turnBtn.interactable = false;
    }
    
    private void OnDisable()
    {
        InGameManager.Instance.OnManaChange -= OnManaChange;
        turnBtn.onClick.RemoveAllListeners();
        pauseBtn.onClick.RemoveAllListeners();
    }

    private void OnTurnBtnClick()
    {
        turnTxt.text = "Enemy Turn";
        turnBtn.interactable = false;
        InGameManager.Instance.SetTurn(GameStateType.CollectingCard);
    }

    private void OnPlayerTurn()
    {
        turnTxt.text = "End Turn";
        turnBtn.interactable = true;
    }

    private void OnManaChange(int value)
    {
        manaTxt.text = value.ToString() + "/" + InGameManager.Instance.TotalMana.ToString();
    }

    // private void ShowAchivement()
    // {
    //    
    //     HideUI(null, true);
    //   
    //     ShowUI(achivementUI);
    //    
    // }

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

   
}
