using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentBehavior : MonoBehaviour
{
    public virtual void LoadComponent()
    {
        
    }

    protected virtual void Reset()
    {
        LoadComponent();
    }

    protected virtual void Awake()
    {
        LoadComponent();
    }

   
}
