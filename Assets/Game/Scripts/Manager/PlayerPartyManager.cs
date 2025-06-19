
using System;
using System.Collections.Generic;
using BrokerChain;
using Cysharp.Threading.Tasks;
using StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPartyManager : Singleton<PlayerPartyManager>
{
    //player must always be at index 0 in the list
   
    [SerializeField] private List<Entity> PlayerPartyPrefabs = new List<Entity>();
    [SerializeField] private List<Vector3> SpawnPositions = new List<Vector3>();

    private List<Entity> playerPartyEntities = new List<Entity>();

    private void OnEnable()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.Win, DespawnAllParty);
        ObserverManager<GameEventType>.Attach(GameEventType.Lose,  DespawnAllParty);
    }

    private void OnDisable()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.Win, DespawnAllParty);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose,  DespawnAllParty);
    }


    public async UniTask SpawnPlayerParty()
    {
       if(playerPartyEntities != null && playerPartyEntities.Count > 0) await SpawnExitPlayerParty();
       else await SpawnNewPlayerParty();
    }

    private async UniTask SpawnExitPlayerParty()
    {
        for (int i = 0; i < playerPartyEntities.Count; ++i)
        {
            Entity entity = playerPartyEntities[i];
            //EntityStats entityStats 
            if(entity == null) continue;
            entity.transform.position = SpawnPositions[i];
            entity.gameObject.SetActive(true);
            
            
            await UniTask.Yield();
            
        }
        
      
    }

    private async UniTask SpawnNewPlayerParty()
    {
        playerPartyEntities.Clear();
        if (PlayerPartyPrefabs.Count == 0)
        {
            Debug.LogWarning("Player party prefabs is null");
            return;
        }

        for (int i = 0; i < PlayerPartyPrefabs.Count; ++i)
        {
            if (i >= SpawnPositions.Count)
            {
                Debug.LogWarning("Spawn position is not enough for player party");
                return;
            }

            Entity entity =  PoolingManager.Spawn(PlayerPartyPrefabs[i].gameObject, SpawnPositions[i], default, transform).GetComponent<Entity>();
            entity.StatsSystem.Init();
            await UniTask.Yield();
           
            playerPartyEntities.Add(entity);
        }
    }
  
    public void DespawnAllParty(object param)
    {
        foreach (Entity entity in playerPartyEntities)
        {
            if(entity != null) entity.gameObject.SetActive(false);
        }

        playerPartyEntities.RemoveAll(t => t == null);
    }

    public Entity GetRandomPartyMember()
    {
        if (playerPartyEntities == null || playerPartyEntities.Count == 0)
        {
            Debug.LogWarning("Player party entity is null");
            return null;
        }

        int index = Random.Range(0, playerPartyEntities.Count);
        return playerPartyEntities[index];
    }

    public Player GetPlayer()
    {
        return (Player)(playerPartyEntities[0]);
    }
}
