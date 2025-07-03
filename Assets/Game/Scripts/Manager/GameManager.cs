         

using UnityEngine;


public class GameManager : PersistentSingleton<GameManager>
{
    public WorldListData WorldListData;
    public int CurrentWorldID;

    public WorldData GetWorldData()
    {
        
        if (WorldListData == null)
        {
            Debug.LogWarning("World List Data in gameManager is null");
            return null;
        }
        
        if (CurrentWorldID >= WorldListData.WorldDatas.Count || CurrentWorldID < 0)
        {
            Debug.LogWarning("World Id is run out of index" );
            return null;
        }

        return WorldListData.WorldDatas[CurrentWorldID];
    }


}
