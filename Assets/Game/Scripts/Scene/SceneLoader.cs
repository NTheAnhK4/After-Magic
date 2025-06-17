
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;

using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : Singleton<SceneLoader>
{
    [SerializeField] private Image panel;
    [SerializeField] private Canvas canvas;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (panel == null) panel = transform.GetComponentInChildren<Image>();
        if (canvas == null) canvas = GetComponent<Canvas>();
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (canvas != null)
        {
            var camera = Camera.main;
            canvas.worldCamera = camera;
        }
        Color color = panel.color;
        color.a = 0;
        
        await panel.DOColor(color, 1.5f).SetUpdate(true).AsyncWaitForCompletion();
        panel.gameObject.SetActive(false);
    }
    public async void LoadScene(string sceneName)
    {
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;
        
        panel.gameObject.SetActive(true);
        Color color = panel.color;
        color.a = 0;
        panel.color = color;

      
        await panel.DOFade(1, .3f).SetUpdate(true).AsyncWaitForCompletion();
       
        while (scene.progress < .9f)
        {
            await UniTask.Delay(
                100,                          
                DelayType.UnscaledDeltaTime
            );
        }

        await UniTask.Delay(
            500,                          
            DelayType.UnscaledDeltaTime
        );

        DOTween.KillAll();
        scene.allowSceneActivation = true;

    }

   
}
