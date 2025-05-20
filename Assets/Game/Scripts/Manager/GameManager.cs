
using System;




public class GameManager : Singleton<GameManager>
{
   
    private GameStateType currentStateType;

    private void Start()
    {
        currentStateType = GameStateType.DistributeCard;
        ObserverManager<GameStateType>.Notify(currentStateType);
    }
    
    public void TakeTurn()
    {
        if (currentStateType == GameStateType.UsingCard) SetTurn(GameStateType.PlayerTurn);
        else if(currentStateType == GameStateType.EnemyTurn) SetTurn(GameStateType.DistributeCard);
        else
        {
            
            int stateInt = ((int)(currentStateType) + 1);
            SetTurn((GameStateType)stateInt);
        }
        
       
    }

    public void SetTurn(GameStateType nextTurn)
    {
        if (currentStateType == nextTurn) return;
        currentStateType = nextTurn;
        ObserverManager<GameStateType>.Notify(currentStateType);
        
    }

    public bool IsTurn(GameStateType turn) => currentStateType == turn;

}
