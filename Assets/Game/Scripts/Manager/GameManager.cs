         
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int CoinAmount = 100;
   
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

   
}
