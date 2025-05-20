
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnEnable()
    {
        if (EnemyManager.Instance == null)
        {
            Debug.Log(null); 
            return;
        }
        EnemyManager.Instance.AddEnemy(this);
    }

    private void OnDisable()
    {
        EnemyManager.Instance.RemoveEnemy(this);
    }

    public async UniTask DoAction()
    {
        Debug.Log(transform.name+" start Acttion");
        await UniTask.Delay(1500);
        Debug.Log(transform.name + " Finish action");
        
    }
}
