         

using SaveGame;
using UnityEngine;


public class GameManager : PersistentSingleton<GameManager>
{
    public WorldListData WorldListData;
    private int currentWorldID;

    public int CurrentWorldID
    {
        get => currentWorldID;
        set
        {
            currentWorldID = value;
            if (SaveLoadSystem.Instance.GameData != null) SaveLoadSystem.Instance.GameData.CurrentWorldId = currentWorldID;
        }
    }

    public void InitData()
    {
        if (SaveLoadSystem.Instance.GameData == null) currentWorldID = 0;
        else currentWorldID = SaveLoadSystem.Instance.GameData.CurrentWorldId;
    }
    public int WorldDataCount => WorldListData != null && WorldListData.WorldDatas != null ? WorldListData.WorldDatas.Count : 0;

    private bool CanGetWorldData(int worldId)
    {
        if (WorldListData == null)
        {
            Debug.LogWarning("World List Data in gameManager is null");
            return false;
        }
        
        if (worldId >= WorldListData.WorldDatas.Count || worldId < 0)
        {
            Debug.LogWarning("World Id is run out of index" );
            return false;
        }

        return true;
    }

    public WorldData GetCurrentWorldData()
    {
        return GetWorldDataById(CurrentWorldID);
    }

    public WorldData GetWorldDataById(int id)
    {
        return CanGetWorldData(id) ? WorldListData.WorldDatas[id] : null;
    }
    


}
