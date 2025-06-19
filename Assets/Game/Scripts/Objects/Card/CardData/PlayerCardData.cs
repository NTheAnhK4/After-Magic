using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Player Card Data", fileName = "Player Card Data")]
public class PlayerCardData : ScriptableObject
{
    public string CardName;
    public CardStrategy CardStrategy;
    public int ManaCost;
    public Sprite CardImage;
    public Sprite CardType;
    public string CardDescription;
}
