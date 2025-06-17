
using System;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using Game.UI;
using UnityEngine;


public class InGameManager : Singleton<InGameManager>
{
    public bool IsGoDeep = false;
    public DungeonRoomType DungeonRoomType;

    public RoomUIBtn CurrentRoom;

    public int CurrentDepth;
    public int MaxDepth;
    
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
        CurrentDepth = 1;
        MaxDepth = 5;
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

   

    public async void PlayGame()
    {
        Time.timeScale = .5f;
        PlayerPartyManager.Instance.SpawnPlayerParty();
        EnemyManager.Instance.SpawnEnemy();
        CardManager.Instance.ClearDesks();
        CardManager.Instance.Init();
       
        TotalMana = 3;
        CurMana = 3;
        IsGameOver = false;
        await UniTask.Delay(300);
        currentStateType = GameStateType.DistributeCard;
        ObserverManager<GameStateType>.Notify(currentStateType);
        
    }
    public void RevivePlayer()
    {
        PlayerPartyManager.Instance.SpawnPlayerParty();
        IsGameOver = false;
    }
}
