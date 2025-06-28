
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Player Card Data", fileName = "Player Card Data")]
public class PlayerCardData : ScriptableObject
{
    public string CardName;
    public Color CardNameColor = Color.white;
    public CardStrategy CardStrategy;
    public int ManaCost;
    public Sprite CardImage;
    public Sprite CardType;
    public string CardDescription;
    public bool IsUnlocked;
}
