
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Item Data", fileName = "Item Data")]
public class ItemData : ScriptableObject
{
    public string IteamName;
    
    public int Amount;
    public string Description;
    public GameObject ItemPrefab;
}
