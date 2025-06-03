
using System;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;


public class InGameManager : Singleton<InGameManager>
{
    public int TotalMana { get; private set; }
    private int curMana;
    public Action<int> OnManaChange;

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

    private void Start()
    {
        PlayGame();
    }

    private void OnEnable()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, param => { IsGameOver = true;});
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
        PlayerPartyManager.Instance.SpawnPlayerParty();
        TotalMana = 3;
        CurMana = 3;
        currentStateType = GameStateType.DistributeCard;
        ObserverManager<GameStateType>.Notify(currentStateType);
        IsGameOver = false;
    }
    public void RevivePlayer()
    {
        PlayerPartyManager.Instance.SpawnPlayerParty();
        IsGameOver = false;
    }
}
