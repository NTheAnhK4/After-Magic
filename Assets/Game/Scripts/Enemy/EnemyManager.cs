
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

    private void OnDisable()
    {
        ObserverManager<GameStateType>.DetachAll();
    }

    public void AddEnemy(Enemy enemy)
    {
        if(enemy == null) return;
        Enemies.Add(enemy);
    }

    public void RemoveEnemy(Enemy enemy)
    {
      
        Enemies.Remove(enemy);
        
    }

    private async void DoEnemyAction()
    {
        foreach (Enemy enemy in Enemies)
        {
            enemy.IsPlanningState = false;
            await enemy.DoAction();
        }
        GameManager.Instance.TakeTurn();
    }

    private void EnemyPlanning()
    {
      
        foreach (Enemy enemy in Enemies)
        {
            enemy.IsPlanningState = true;
        }
    }

    
}
