using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEffectCtrl : ComponentBehaviour
{
    private WorldEffect worldEffect;

    public void Init(WorldEffect effect = null)
    {
        worldEffect = effect;
        if(worldEffect != null) worldEffect.RegisterEnvent();
    }
    
    private void OnDisable()
    {
        if(worldEffect != null) worldEffect.UnRegisterEvent();
    }
}
