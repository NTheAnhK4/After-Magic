
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace System.Persistence
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
        [SerializeField] public GameData gameData;
        private IDataService dataService;
        protected override void Awake()
        {
            base.Awake();
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
            gameData = new GameData()
            {
                Name = "new Game" ,
                CurrentLevelName = GameConstants.LobbyScene
            };
            SceneLoader.Instance.LoadScene(gameData.CurrentLevelName);
        }

        public void SaveGame()
        {
            dataService.Save(gameData);
        }

        public bool CanLoadFile(string gameName) => dataService.CanLoad(gameName);

        public void LoadGame(string gameName)
        {
            gameData = dataService.Load(gameName);
            if (String.IsNullOrWhiteSpace(gameData.CurrentLevelName))
            {
                gameData.CurrentLevelName = GameConstants.LobbyScene;
            }
            //SceneLoader.Instance.LoadScene(gameData.CurrentLevelName);
        }

        public void DeleteGame(string gameName) => dataService.Delete(gameName);
        public void ReloadGame() => LoadGame(gameData.Name);
    }
}
