
using System.Collections.Generic;

using StateMachine;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Party Data", fileName = "Party Data")]
public class PartyData : ScriptableObject
{
    public List<Entity> PartyPrefabs;
    public Entity GetPartyById(int id) => PartyPrefabs[id];
}
