
using System.Collections.Generic;
using BrokerChain.Status;

using UnityEngine;


public class StatusUICtrl : ComponentBehaviour
{
    

    class IconInfor
    {
        public StatusIconCtrl IconPrefab;
        public StatusEffectData Data;
        public int Amount = 0;
    }
    public StatusIconCtrl StatusIcon;
    private Dictionary<StatusEffectType, IconInfor> iconInfors = new();
   
    public void AddEffect(StatusEffectData statusEffectData)
    {
        if (statusEffectData == null || statusEffectData.Icon == null) return;

        StatusEffectType statusEffectType = statusEffectData.StatusEffectType;
       
        if (iconInfors.TryGetValue(statusEffectType, out IconInfor iconInfor))
        {
          
            iconInfor.Amount++;
            iconInfor.IconPrefab.Init(null, iconInfor.Amount.ToString());
        }
        else
        {
           
            GameObject iconGO = PoolingManager.Spawn(StatusIcon.gameObject,transform);
            if (iconGO == null) return;
            StatusIconCtrl statusIconCtrl = iconGO.GetComponent<StatusIconCtrl>();
            if (statusIconCtrl == null) return;
            statusIconCtrl.Init(statusEffectData.Icon,"1");
            IconInfor newIconInfor = new IconInfor()
            {
                IconPrefab = statusIconCtrl,
                Data = statusEffectData,
                Amount = 1
            };
            iconInfors.Add(statusEffectType, newIconInfor);
        }
        
     
    }

    public void RemoveEffect(StatusEffectData statusEffectData)
    {
        StatusEffectType statusEffectType = statusEffectData.StatusEffectType;
        if (iconInfors.TryGetValue(statusEffectType, out IconInfor iconInfor))
        {
            iconInfor.Amount--;
            if (iconInfor.Amount <= 0)
            {
                PoolingManager.Despawn(iconInfor.IconPrefab.gameObject);
                iconInfors.Remove(statusEffectType);
            }
            else
            {
                iconInfor.IconPrefab.Init(null, iconInfor.Amount.ToString());
            }
           
        }
       
    }

    private void OnEnable()
    {
        foreach (var value in iconInfors.Values)
        {
            if(value == null) continue;
            PoolingManager.Despawn(value.IconPrefab.gameObject);
        }
        iconInfors.Clear();
    }
}
