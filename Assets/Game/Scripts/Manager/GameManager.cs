
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
        int stateInt = ((int)(currentStateType) + 1) % Enum.GetValues(typeof(GameStateType)).Length;

        currentStateType = (GameStateType)stateInt;
       
        ObserverManager<GameStateType>.Notify(currentStateType);
    }
    
}
