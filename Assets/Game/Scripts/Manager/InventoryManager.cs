using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{
    public Dictionary<ItemData, int> EquippedItems = new Dictionary<ItemData, int>();
    public Dictionary<ItemData, int> DungeonLoot = new Dictionary<ItemData, int>();

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    public void AddToLoot(ItemData itemStrategy)
    {
        if (DungeonLoot.ContainsKey(itemStrategy)) DungeonLoot[itemStrategy] += itemStrategy.Amount;
        else DungeonLoot[itemStrategy] = itemStrategy.Amount;
       
    }

    public void SetDungeonLootPercentage(int percent)
    {
        foreach (var key in DungeonLoot.Keys.ToList())
        {
            DungeonLoot[key] = DungeonLoot[key] * percent / 100;
        }
        
    }

    public void MoveLootToInventory()
    {
        foreach (var (key, value) in DungeonLoot)
        {
            EquippedItems[key] += value;
        }
        
        DungeonLoot.Clear();
    }

    public void DiscardLoot() => DungeonLoot.Clear();
}