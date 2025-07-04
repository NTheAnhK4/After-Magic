
using Cysharp.Threading.Tasks;

using DG.Tweening;
using UnityEngine;

using UnityEngine.SceneManagement;


public class SceneLoader : ComponentBehaviour
{
    [SerializeField] private CanvasGroup panel;
    private static SceneLoader instance;
  
    public static SceneLoader Instance => instance;

    protected override void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else Destroy(gameObject);

        base.Awake();
       
        
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public override void LoadComponent()
    {
        base.LoadComponent();
        
        if (panel == null) panel = transform.GetComponentInChildren<CanvasGroup>();
        
    }

    private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        await UniTask.Delay(1, DelayType.UnscaledDeltaTime);
        LoadComponent();
        IGameInitializer gameInitializer = FindAnyObjectByType<IGameInitializer>();

        if (gameInitializer != null) await gameInitializer.Init();

        await panel.DOFade(0, .5f).SetUpdate(true).AsyncWaitForCompletion();

        panel.blocksRaycasts = false;
    }
    public async void LoadScene(string sceneName)
    {
        if (panel == null)
        {
            Debug.LogError("SceneLoader: panel is null!");
            return;
        }
        if(MusicManager.Instance != null) MusicManager.Instance.StopMusic();
        
        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        panel.blocksRaycasts = true;
        await panel.DOFade(1, .5f).SetUpdate(true).AsyncWaitForCompletion();
       
        

      
      
       
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

        DOTween.Kill(panel);
        scene.allowSceneActivation = true;

    }

   
}
