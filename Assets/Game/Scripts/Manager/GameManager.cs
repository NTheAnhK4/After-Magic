         
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private static string currentScene = GameConstants.DungeonScene;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public static void LoadScene(string sceneName)
    {
        DOTween.KillAll();

        if (currentScene == GameConstants.DungeonScene)
        {
            ObserverManager<GameStateType>.DetachAll();
            ObserverManager<CardTargetType>.DetachAll();
            ObserverManager<GameEventType>.DetachAll();
        }
        SceneManager.LoadScene(sceneName);
        currentScene = sceneName;
    }
}
