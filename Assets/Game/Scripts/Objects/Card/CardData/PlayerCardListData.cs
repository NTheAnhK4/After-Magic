
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/Card/Player Card List Data", fileName = "Player Card List Data")]
public class PlayerCardListData : ScriptableObject
{
    public List<PlayerCardData> CardDesks = new List<PlayerCardData>();
    public List<PlayerCardData> UnlockedCards { get; private set; }

    private bool needsRecaculation = true;
    public void MarkDirty() => needsRecaculation = true;

    public List<PlayerCardData> GetUnlockedCards()
    {
        if(needsRecaculation) RecaculateUnlockedCards();
        return UnlockedCards;
    }

    private void RecaculateUnlockedCards()
    {
        UnlockedCards = CardDesks.Where(card => card.IsUnlocked).ToList();
        needsRecaculation = false;
    }

    public void UnlockCard(PlayerCardData cardData)
    {
        if (!cardData.IsUnlocked)
        {
            cardData.IsUnlocked = true;
            MarkDirty();
        }
    }

    public PlayerCardData GetRandomCard()
    {
        GetUnlockedCards();
        int cardId = Random.Range(0, UnlockedCards.Count);
        return UnlockedCards[cardId];
    }

    public PlayerCardData GetPlayerCardDataByID(int id)
    {
        return CardDesks.FirstOrDefault(t => t.ID == id);
    }
    [ContextMenu("Auto Assign IDs")]
    public void AutoAssignIds()
    {
        for (int i = 0; i < CardDesks.Count; ++i)
        {
            if (CardDesks[i] != null)
            {
                CardDesks[i].ID = i;
                #if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(CardDesks[i]);
                #endif
                }
            }
            #if UNITY_EDITOR
                    UnityEditor.AssetDatabase.SaveAssets();
            #endif
    }

}
