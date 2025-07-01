using System;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Item/Item Prefab List Data", fileName = "Item Prefab List")]
public class ItemPrefabListData : ScriptableObject
{
    public List<ItemPrefabInfor> ItemPrefabInfors;

    public GameObject GetItemPrefabByType(ItemType itemType)
    {
        return ItemPrefabInfors.FirstOrDefault(t => t.ItemType == itemType)?.ItemPrefab;
    }
}

[Serializable]
public class ItemPrefabInfor
{
    public GameObject ItemPrefab;
    public ItemType ItemType;
}
