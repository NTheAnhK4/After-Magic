
using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class EnemyManager : Singleton<EnemyManager>
{
    
    [SerializeField] private RoomEnemyGroupConfig Data;

    [SerializeField] private List<Vector3> SpawnPos = new List<Vector3>();
    [SerializeField] private List<Enemy> Enemies = new List<Enemy>();

    #region Action

    private Action<object> onEnemyTurnAction;
    private Action<object> onPlayerTurnAction;

    #endregion
    private void OnEnable()
    {
        WorldData worldData = GameManager.Instance.GetWorldData();
        if (worldData != null && worldData.RoomEnemyGroupConfig != null) Data = worldData.RoomEnemyGroupConfig;
        
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
        if (Enemies.Remove(enemy))
        {
            if (InGameManager.Instance != null)
            {
                //switch case her 
                InGameManager.Instance.MonstersDefeated++;
            }
        }
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

    public async UniTask SpawnEnemy()
    {
        
        if (Data == null)
        {
            Debug.LogWarning("Data for enemy spawning is null");
            return;
        }

        EnemyGroup enemyGroup = Data.GetRandomGroupByFloor(InGameManager.Instance.CurrentDepth, InGameManager.Instance.MaxDepth, InGameManager.Instance.CurrentRoom.DungeonRoomType is DungeonRoomType.Door);

     
        if (enemyGroup == null || enemyGroup.Enemies == null)
        {
            Debug.LogWarning("Can not caculate enemy group");
            return;
        }

        for (int i = 0; i < enemyGroup.Enemies.Count; ++i)
        {
            Enemy prefab = enemyGroup.Enemies[i];
            if(prefab == null) Debug.LogWarning("Prefab is null");
            if(prefab == null) continue;
            if (i >= SpawnPos.Count)
            {
                Debug.LogWarning("Spawn pos for enemy spawning is not enough");
                return;
            }

            Enemy enemy = PoolingManager.Spawn(prefab.gameObject, SpawnPos[i], default, transform).GetComponent<Enemy>();
            if(enemy != null) AddEnemy(enemy);
            await UniTask.Delay(200, DelayType.UnscaledDeltaTime);
        }
        
     
    }
    
}
