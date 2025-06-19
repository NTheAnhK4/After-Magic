
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Enemy Card Data", fileName = "Enemy Card Data")]
public class EnemyCardData : ScriptableObject
{
    public GameObject WarningPrefab;
    public CardStrategy CardStrategy;
}
