
using System;
using System.Collections.Generic;

using BrokerChain;
using Cysharp.Threading.Tasks;
using SaveGame;
using StateMachine;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerPartyManager : Singleton<PlayerPartyManager>
{
   
    public PartyData PartyData;

    [SerializeField] private List<int> partyId = new List<int>();
    
    [SerializeField] private List<Vector3> SpawnPositions = new List<Vector3>();

    private List<Entity> playerPartyEntities = new List<Entity>();
    public List<EntityStatsSaveData> EntityStatsSaveDatas;

    private void OnEnable()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.Win, DespawnAllParty);
        ObserverManager<GameEventType>.Attach(GameEventType.Lose, DespawnAllParty);
    }

    private void OnDisable()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.Win, DespawnAllParty);
        ObserverManager<GameEventType>.Detach(GameEventType.Lose, DespawnAllParty);
    }

    public void InitData()
    {
        if (SaveLoadSystem.Instance.GameData == null) return;
        EntitiesStatsSaveData entitiesStatsSaveData = SaveLoadSystem.Instance.GameData.EntitiesStatsSaveData ?? new EntitiesStatsSaveData();
        if (entitiesStatsSaveData.EntityStatsSaveDatas == null || entitiesStatsSaveData.EntityStatsSaveDatas.Count == 0)
        {
            entitiesStatsSaveData.EntityStatsSaveDatas = new List<EntityStatsSaveData>();
        }

        

        EntityStatsSaveDatas = entitiesStatsSaveData.EntityStatsSaveDatas;
        
    }

    public async UniTask SpawnPlayerParty()
    {
       
        if (playerPartyEntities is { Count: > 0 }) await SpawnExitPlayerParty();
        else await SpawnNewPlayerParty(EntityStatsSaveDatas);
    }

  
    private async UniTask SpawnExitPlayerParty()
    {
        for (int i = 0; i < playerPartyEntities.Count; ++i)
        {
            Entity entity = playerPartyEntities[i];
            
            EntityStats entityStats = entity.StatsSystem.Stats?.EntityStats;
            if(entity == null) continue;
            entity.transform.position = SpawnPositions[i];
            entity.gameObject.SetActive(true);
            entity.StatsSystem.Init(entityStats);

            await UniTask.Delay(200, DelayType.UnscaledDeltaTime);

        }
        
      
    }

    public async UniTask SpawnNewPlayerParty(List<EntityStatsSaveData> entityStatsSaveDatas = null)
    {
        playerPartyEntities.Clear();
        if (PartyData == null)
        {
            Debug.LogError("Party Data is null");
            return;
        }
        if (partyId.Count == 0)
        {
            Debug.LogWarning("Player party prefabs is null");
            return;
        }

        if (entityStatsSaveDatas == null) entityStatsSaveDatas = new List<EntityStatsSaveData>();
        for (int i = 0; i < partyId.Count; ++i)
        {
            if (i >= SpawnPositions.Count)
            {
                Debug.LogWarning("Spawn position is not enough for player party");
                return;
            }

            Entity entityPrefab = PartyData.GetPartyById(partyId[i]);
            Entity entity =  PoolingManager.Spawn(entityPrefab.gameObject, SpawnPositions[i], default, transform).GetComponent<Entity>();
            if (entityStatsSaveDatas.Count > i)
            {
                EntityStats entityStats = new EntityStats()
                {
                    HP = entityStatsSaveDatas[i].CurHp,
                    MaxHP = entityStatsSaveDatas[i].MaxHp,
                    Damage = entityStatsSaveDatas[i].Damage,
                    Defense = entityStatsSaveDatas[i].Defense,
                    ExtraTakenDamage = 0,
                };
                entity.StatsSystem.Init(entityStats);
                
            }
            else
            {
                entity.StatsSystem.Init();
                EntityStats entityStats = entity.StatsSystem.Stats.EntityStats;
                EntityStatsSaveData entityStatsSaveData = new EntityStatsSaveData()
                {

                    CurHp = entityStats.HP,
                    MaxHp = entityStats.MaxHP,
                    Defense = entityStats.Defense,
                    ExtraTakenDamage = entityStats.ExtraTakenDamage,
                    Damage = entityStats.Damage
                };
                entityStatsSaveDatas.Add(entityStatsSaveData);
            }
            var i3 = i;
            entity.StatsSystem.Stats.EntityStats.OnHPChange += (i1, _) =>
            {
                entityStatsSaveDatas[i3].CurHp = i1;
            };

           
            
            await UniTask.Delay(200, DelayType.UnscaledDeltaTime);
           
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

[Serializable]
public class EntitiesStatsSaveData : ISaveable
{
    public SerializableGuid Id { get; set; }
    public List<EntityStatsSaveData> EntityStatsSaveDatas = new List<EntityStatsSaveData>();
    public List<int> PartyId = new List<int>();

    
}

[Serializable]
public class EntityStatsSaveData
{
    public int MaxHp;
    public int Defense;
    public int Damage;

    public int ExtraTakenDamage = 0;
    public int CurHp;
}
