using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    class InventoryItem
    {
        public GameObject ItemPrefab { get; set; }
        public int Amount { get; set; }
    }
    private Dictionary<ItemType, InventoryItem> EquippedItems = new Dictionary<ItemType, InventoryItem>();
    private Dictionary<ItemType, InventoryItem> DungeonLoot = new Dictionary<ItemType, InventoryItem>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void AddToLoot(ItemType itemType ,GameObject itemPrefab, int amount)
    {
        if (!DungeonLoot.ContainsKey(itemType))
        {
            InventoryItem inventoryItem = new InventoryItem()
            {
                ItemPrefab = itemPrefab,
                Amount = amount
            };
            DungeonLoot.Add(itemType, inventoryItem);
        }
        else
        {
            InventoryItem inventoryItem = DungeonLoot[itemType];
            inventoryItem.Amount += amount;
        }
        
       
    }

    public void SetDungeonLootPercentage(int percent)
    {
        foreach (var key in DungeonLoot.Keys.ToList())
        {
            DungeonLoot[key].Amount = DungeonLoot[key].Amount * percent / 100;
        }
        
    }

    public void MoveLootToInventory()
    {
        foreach (var (key, value) in DungeonLoot)
        {
            if(!EquippedItems.ContainsKey(key)) EquippedItems.Add(key,value);
            else EquippedItems[key].Amount += value.Amount;
        }
        
        DungeonLoot.Clear();
    }

    public void DiscardLoot() => DungeonLoot.Clear();

    public async UniTask<List<ItemBase>> ShowItemInLoot(Transform itemHolder = null)
    {
        List<ItemBase> itemBases = new List<ItemBase>();
        
        foreach (var (key, value) in DungeonLoot)
        {
            ItemBase itemBase = PoolingManager.Spawn(value.ItemPrefab, itemHolder).GetComponent<ItemBase>();
            itemBase.SetInteracable(false);
          
            itemBases.Add(itemBase);
            await itemBase.ShowReward(value.Amount);
        }
        
        return itemBases;
    }
}