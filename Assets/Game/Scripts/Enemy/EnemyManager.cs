
using System.Collections.Generic;


public class EnemyManager : Singleton<EnemyManager>
{
    private List<Enemy> Enemies = new List<Enemy>();
    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.EnemyTurn, param => DoEnemyAction());
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
            await enemy.DoAction();
        }
        GameManager.Instance.TakeTurn();
    }

    
}
