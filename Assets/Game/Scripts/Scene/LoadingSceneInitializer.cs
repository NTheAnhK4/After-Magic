

using AudioSystem;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using SaveGame;
using UnityEngine;


public class LoadingSceneInitializer : IGameInitializer
{
    [SerializeField] private LoadingScreenUI loadingScreenUI;
    [SerializeField] private SaveLoadSystem saveLoadSystemPrefab;
    [SerializeField] private SoundManager soundManagerPrefab;
    [SerializeField] private MusicManager musicManagerPrefab;

    [SerializeField] private InventoryManager inventoryManagerPrefab;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (loadingScreenUI == null) loadingScreenUI = FindObjectOfType<LoadingScreenUI>();

    }

  

    public override async UniTask Init()
    {
        if (loadingScreenUI == null)
        {
            Debug.LogError("Cannot find any LoadingScreenUI");
    
            return;
        }
    
        loadingScreenUI.IsDataLoaded = false;
        if (saveLoadSystemPrefab == null)
        {
            Debug.LogError("Save Load Sytem is null");
            return;
        }
    
        SaveLoadSystem saveLoadSystem = Instantiate(saveLoadSystemPrefab);
        await loadingScreenUI.slider.DOValue(0.2f, 0.15f).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    
        if (saveLoadSystem.GameData == null)
        {
            saveLoadSystem.GameData = new GameData();
            saveLoadSystem.GameData.Name = "Game";
            saveLoadSystem.GameData.CurrentLevelName = GameConstants.LobbyScene;
        }
        if (saveLoadSystem.CanLoadFile(saveLoadSystem.GameData.Name))
        {
            saveLoadSystem.LoadGame(saveLoadSystem.GameData.Name);
            loadingScreenUI.IsDataLoaded = true;
        }
       
        await loadingScreenUI.slider.DOValue(0.4f, 0.3f).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    
    
        
        if (soundManagerPrefab == null) Debug.LogWarning("Sound Manager prefab is null");
        else Instantiate(soundManagerPrefab);
    
        if (musicManagerPrefab == null) Debug.LogWarning("Music manager prefab is null");
        else Instantiate(musicManagerPrefab);
    
        await loadingScreenUI.slider.DOValue(0.6f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    
        if (inventoryManagerPrefab == null)
        {
            Debug.LogError("Inventory Manager prefab is null");
            return;
        }
    
        InventoryManager inventoryManager = Instantiate(inventoryManagerPrefab);
    
        await loadingScreenUI.slider.DOValue(0.7f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
        
        
        inventoryManager.Bind(saveLoadSystem.GameData.InventorySaveData);
        await loadingScreenUI.slider.DOValue(1f, 0.2f).SetEase(Ease.InOutSine).SetUpdate(true).AsyncWaitForCompletion();
    
        
    }
    
}
