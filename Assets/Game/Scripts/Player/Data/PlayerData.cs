using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Player/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("Run State")] public float RunSpeed = 5f;
    [Header("Attack State")] public float AttackRange = 1f;
}
