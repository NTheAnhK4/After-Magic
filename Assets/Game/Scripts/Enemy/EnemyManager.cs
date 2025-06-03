
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private List<Enemy> Enemies = new List<Enemy>();
    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, param => DoEnemyAction());
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, param => EnemyPlanning());
    }

  

    public void AddEnemy(Enemy enemy)
    {
        if(enemy == null) return;
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
        Enemies.Remove(enemy);
        if (Enemies.Count == 0 && !InGameManager.Instance.IsGameOver) ObserverManager<GameEventType>.Notify(GameEventType.Win);
    }

    private async void DoEnemyAction()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.IsPlanningState = false;
            await enemy.DoAction();
        }
        InGameManager.Instance.SetTurn(GameStateType.DistributeCard);
    }

    private void EnemyPlanning()
    {
      
        foreach (Enemy enemy in Enemies)
        {
            enemy.IsPlanningState = true;
        }
    }

    
}
