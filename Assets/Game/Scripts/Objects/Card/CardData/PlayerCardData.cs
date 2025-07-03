
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Card/Player Card Data", fileName = "Player Card Data")]

public class PlayerCardData : ScriptableObject
{
    public int ID;
    public string CardName;
    public Color CardNameColor = Color.white;
    public CardStrategy CardStrategy;
    public int ManaCost;
    public Sprite CardImage;
    public Sprite CardTypeSprite;
    public string CardDescription;
    public bool IsUnlocked;
    public CardType CardType;
}
