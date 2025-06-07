
using System;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPartyManager : Singleton<PlayerPartyManager>
{
    //player must always be at index 0 in the list
    [SerializeField] private List<GameObject> PlayerPartyPrefabs = new List<GameObject>();
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


    public void SpawnPlayerParty()
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

            Entity entity =  PoolingManager.Spawn(PlayerPartyPrefabs[i], SpawnPositions[i], default, transform).GetComponent<Entity>();
            playerPartyEntities.Add(entity);
        }
    }

  
    public void DespawnAllParty(object param)
    {
        foreach (Entity entity in playerPartyEntities)
        {
            if(entity != null) PoolingManager.Despawn(entity.gameObject);
        }
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
