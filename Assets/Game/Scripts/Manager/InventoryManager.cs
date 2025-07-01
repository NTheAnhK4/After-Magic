using System;
using System.Collections.Generic;
using System.Linq;
using System.Persistence;
using Cysharp.Threading.Tasks;

using UnityEngine;


public enum ItemType
{
    Coin,
}

public class InventoryManager : PersistentSingleton<InventoryManager>, IBind<InventorySaveData>
{
    public ItemPrefabListData ItemPrefabListData;
    [SerializeField] private List<ItemSaveData> DungeonLoot = new List<ItemSaveData>();
    [SerializeField] private List<ItemSaveData> EquippedItems = new List<ItemSaveData>();
   
    public SerializableGuid Id { get; set; }
    public InventorySaveData InventorySaveData;

    public int GetAmountFromLoot(ItemType itemType)
    {
        ItemSaveData itemSaveData = DungeonLoot.FirstOrDefault(t => t.ItemType == itemType);
        return itemSaveData == null ? 0 : itemSaveData.Amount;
    }

    public int GetAmountFromEquippedItems(ItemType itemType)
    {
        ItemSaveData itemSaveData = EquippedItems.FirstOrDefault(t => t.ItemType == itemType);
        return itemSaveData == null ? 0 : itemSaveData.Amount;
    }
   
    public void AddToLoot(ItemType itemType , int amount)
    {
        ItemSaveData itemSaveData = DungeonLoot.FirstOrDefault(t => t.ItemType == itemType);
        if (itemSaveData != null) itemSaveData.Amount += amount;
        else DungeonLoot.Add(new ItemSaveData(){ItemType = itemType,Amount = amount});
        

    }

    public void SetDungeonLootPercentage(int percent)
    {
        foreach (var item in DungeonLoot)
        {
            if(item == null) continue;
            item.Amount = item.Amount * percent / 100;
        }
       
       
        
    }

    public void MoveLootToInventory()
    {
        foreach (var item in DungeonLoot)
        {
            var itemSaveData = EquippedItems.FirstOrDefault(t => t.ItemType == item.ItemType);
            if (itemSaveData != null) itemSaveData.Amount += item.Amount;
            else EquippedItems.Add(item);
        }
       
        
        DungeonLoot.Clear();
       
    }

    public void DiscardLoot() => DungeonLoot.Clear();

    public async UniTask<List<ItemBase>> ShowItemInLoot(Transform itemHolder = null)
    {
        if (ItemPrefabListData == null)
        {
            Debug.LogWarning("ItemPrefabListData is null");
            return null;
        }
        List<ItemBase> itemBases = new List<ItemBase>();

        foreach (var item in DungeonLoot)
        {
            if(item == null) continue;
            GameObject itemPrefab = ItemPrefabListData.GetItemPrefabByType(item.ItemType);
            if (itemPrefab == null)
            {
                Debug.LogWarning($"Item prefab for '{item.ItemType}' is null");
                continue;
            }
            ItemBase itemBase = PoolingManager.Spawn(itemPrefab, itemHolder).GetComponent<ItemBase>();
            itemBase.SetInteracable(false);
          
            itemBases.Add(itemBase);
            await itemBase.ShowReward(item.Amount);
        }
       
        
        return itemBases;
    }

    
    public void Bind(InventorySaveData data)
    {
        this.InventorySaveData = data;
        data ??= new InventorySaveData();
        this.DungeonLoot = data.DungeonLootItem;
        this.EquippedItems = data.EquippedItems;
    }
}
[Serializable]
public class InventorySaveData : ISaveable
{
    public SerializableGuid Id { get; set; }
    public List<ItemSaveData> DungeonLootItem;
    public List<ItemSaveData> EquippedItems;
    public InventorySaveData()
    {
        DungeonLootItem = new List<ItemSaveData>();
        EquippedItems = new List<ItemSaveData>();
    }
}

[Serializable]
public class ItemSaveData
{
    public ItemType ItemType;
    public int Amount;
}