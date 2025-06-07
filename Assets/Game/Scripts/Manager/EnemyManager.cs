
using System;
using System.Collections.Generic;
using UnityEngine;


public class EnemyManager : Singleton<EnemyManager>
{
    [SerializeField] private List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] private List<Enemy> Enemies = new List<Enemy>();

    #region Action

    private Action<object> onEnemyTurnAction;
    private Action<object> onPlayerTurnAction;

    #endregion
    private void OnEnable()
    {
        onEnemyTurnAction = param => DoEnemyAction();
        onPlayerTurnAction = param => EnemyPlanning();
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, onEnemyTurnAction);
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, onPlayerTurnAction);
    }

    private void OnDisable()
    {
        ObserverManager<GameStateType>.Detach(GameStateType.EnemyTurn, onEnemyTurnAction);
        ObserverManager<GameStateType>.Detach(GameStateType.PlayerTurn, onPlayerTurnAction);
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

    public void SpawnEnemy()
    {
        Enemy enemy = PoolingManager.Spawn(enemyPrefabs[0], new Vector3(4, 0, 0)).GetComponent<Enemy>();
        AddEnemy(enemy);
        enemy =  PoolingManager.Spawn(enemyPrefabs[0], new Vector3(9, 0, 0)).GetComponent<Enemy>();
        AddEnemy(enemy);
    }
    
}
