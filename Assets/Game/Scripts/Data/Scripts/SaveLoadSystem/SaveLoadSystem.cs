
using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;
namespace SaveGame
{
    [Serializable]
    public class GameData
    {
        public string Name;
        public string CurrentLevelName;

        public DungeonSaveData DungeonSaveData;
        public InventorySaveData InventorySaveData;
        public EntitiesStatsSaveData EntitiesStatsSaveData;

        
        public void ExitDungeon()
        {
            CurrentLevelName = GameConstants.LobbyScene;
            DungeonSaveData = new DungeonSaveData();
            EntitiesStatsSaveData = new EntitiesStatsSaveData();
        }
    }
    
    

    public interface ISaveable
    {
        SerializableGuid Id { get; set; }
    }

    public interface IBind<TData> where TData : ISaveable
    {
        SerializableGuid Id { get; set; }
        void Bind(TData data);

    }
    public class SaveLoadSystem : PersistentSingleton<SaveLoadSystem>
    {
        public GameData GameData;
        private IDataService dataService;
        protected override void Awake()
        {
            base.Awake();
            GameData = new GameData();
            GameData.Name = "Game";
            dataService = new FileDataService(new JsonSerializer());
        }

        void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
           
        }
        void Bind<T, TData>(TData data) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entity = FindObjectsByType<T>(FindObjectsSortMode.None).FirstOrDefault();
            if (entity != null)
            {
                if (data == null) data = new TData(){Id = entity.Id};
                entity.Bind(data);
            }
        }

        

        void Bind<T, TData>(List<TData> datas) where T : MonoBehaviour, IBind<TData> where TData : ISaveable, new()
        {
            var entities = FindObjectsByType<T>(FindObjectsSortMode.None);
            foreach (var entity in entities)
            {
                var data = datas.FirstOrDefault(d => d.Id.Equals(entity.Id));
                if (data == null)
                {
                    data = new TData { Id = entity.Id };
                    datas.Add(data);
                }
                entity.Bind(data);
            }
        }
        public void NewGame()
        {
            GameData = new GameData()
            {
                Name = "New Game" ,
                CurrentLevelName = GameConstants.LobbyScene
            };
            if (SceneLoader.Instance != null)
            {
                SceneLoader.Instance.LoadScene(GameData.CurrentLevelName);
            }
            else
            {
                Debug.LogError("SceneLoader.Instance is null — cannot load scene.");
            }

        }

        public void SaveGame()
        {
            if (dataService == null)
            {
                Debug.LogError("DataService is null — cannot save game.");
                return;
            }

            dataService.Save(GameData);
        }

        public bool CanLoadFile(string gameName) => dataService.CanLoad(gameName);

        public void LoadGame(string gameName)
        {
            if (dataService == null)
            {
                Debug.LogError("DataService is null — cannot save game.");
                return;
            }

            try
            {
                GameData = dataService.Load(gameName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to load game data: {ex.Message}");
                GameData = new GameData()
                {
                    Name = gameName,
                    CurrentLevelName = GameConstants.LobbyScene
                };
            }
            if (String.IsNullOrWhiteSpace(GameData.CurrentLevelName))
            {
                GameData.CurrentLevelName = GameConstants.LobbyScene;
            }
            
        }

        public void DeleteGame(string gameName) => dataService.Delete(gameName);
        public void ReloadGame()
        {
            if (string.IsNullOrWhiteSpace(GameData?.Name))
            {
                Debug.LogWarning("GameData name is null or empty — cannot reload.");
                return;
            }

            LoadGame(GameData.Name);
        }
    }
}
