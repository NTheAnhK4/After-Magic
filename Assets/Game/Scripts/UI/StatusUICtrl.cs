using System.Collections;
using System.Collections.Generic;
using BrokerChain.Status;
using UnityEngine;
using UnityEngine.UI;

public class StatusUICtrl : ComponentBehavior
{
    public Image StatusIcon;
    private Dictionary<StatusEffectData, Image> iconDict = new();

    public void AddEffect(StatusEffectData statusEffectData)
    {
       
        if (statusEffectData == null 
            || iconDict.ContainsKey(statusEffectData)
            || statusEffectData.Icon == null) return;
        if (StatusIcon == null)
        {
            Debug.LogWarning("Status icon prefab is null");
            return;
        }
        Image icon = PoolingManager.Spawn(StatusIcon.gameObject, transform).GetComponent<Image>();
        icon.sprite = statusEffectData.Icon;
        iconDict.Add(statusEffectData, icon);
    }

    public void RemoveEffect(StatusEffectData statusEffectData)
    {
        if (!iconDict.ContainsKey(statusEffectData)) return;
        PoolingManager.Despawn(iconDict[statusEffectData].gameObject);
        iconDict.Remove(statusEffectData);
    }
}
