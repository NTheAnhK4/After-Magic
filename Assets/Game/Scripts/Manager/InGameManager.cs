
using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Game.UI;
using UnityEngine;
using Random = UnityEngine.Random;


public class InGameManager : Singleton<InGameManager>
{
    [Header("Achivement")]
    public int CurrentDepth;
    public int MaxDepth;

    public int MonstersDefeated;
    public int EliteMonstersDefeated;
    public int BossesDefeated;
    public int RoomsExplored;
    public float TimePlayed => Time.time - startTime;

    private float startTime;
    
    public bool IsGoDeep = false;
    public DungeonRoomType DungeonRoomType;

    public RoomUIBtn CurrentRoom;

   
    
    public int TotalMana { get; private set; }
    private int curMana;
    public Action<int> OnManaChange;
    private Action<object> onLoseAction;
    private Action<object> onWinAction;

    public int CurMana
    {
        get => curMana;
        set
        {
            if (curMana != value)
            {
                curMana = value;
                OnManaChange?.Invoke(value);
            }
            
        }
    }
  
    [SerializeField] private GameStateType currentStateType;
    public bool IsGameOver;
    
    private void OnEnable()
    {
       
        MaxDepth = Random.Range(2, 5);
        onLoseAction = param => { IsGameOver = true; };
        onWinAction = param =>
        {
            if (CurrentRoom != null) CurrentRoom.SetInteracableNeighboringRoom();
        };
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, onLoseAction);
        ObserverManager<GameEventType>.Attach(GameEventType.Win, onWinAction);
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        ObserverManager<GameEventType>.Detach(GameEventType.Win, onWinAction);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose, onLoseAction);
    }

    #region Turn

   
    public void SetTurn(GameStateType nextTurn)
    {
        if (currentStateType == nextTurn) return;
        if (nextTurn == GameStateType.DistributeCard) CurMana = TotalMana;
       
        currentStateType = nextTurn;
        ObserverManager<GameStateType>.Notify(currentStateType);
        
    }

    public bool IsTurn(GameStateType turn) => currentStateType == turn;
  
    #endregion

    public bool CanUseMana(int value) => curMana >= value;
    public void TakeMana(int value) => CurMana -= value;

    public void PlayGame()
    {
        startTime = Time.time;
        CurrentDepth = 1;
        MonstersDefeated = 0;
        EliteMonstersDefeated = 0;
        RoomsExplored = 0;
    }

    private void Start()
    {
        PlayGame();
       
    }

    public async void EnterBattle()
    {
        
        await UniTask.WhenAll(
            PlayerPartyManager.Instance.SpawnPlayerParty(),
            EnemyManager.Instance.SpawnEnemy());
       
        CardManager.Instance.ClearDesks();
        CardManager.Instance.Init();
       
        TotalMana = 3;
        CurMana = 3;
        IsGameOver = false;
        await UniTask.Delay(300, DelayType.UnscaledDeltaTime);
        await UIScreen.Instance.HidePanel();
      
        currentStateType = GameStateType.DistributeCard;
        ObserverManager<GameStateType>.Notify(currentStateType);
        
    }
    public async void RevivePlayer()
    {
        await PlayerPartyManager.Instance.SpawnNewPlayerParty();
        IsGameOver = false;
    }
}
