using System;
using UnityEngine;
using UnityEngine.UI;

public class GlowColorController : ComponentBehaviour
{
    
    
    [ColorUsage(true, true)] 
    [SerializeField] private Color glowColor = Color.white;

    [SerializeField] private Material _material;
    [Range(0, 4)] [SerializeField] private float _outlineThickness;
   

    public override void LoadComponent()
    {
        base.LoadComponent();
        if (_material == null)
        {
            _material =  GetComponent<Image>().material;
        }

        Init();
    }

    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        _material.SetColor(Shader.PropertyToID("_OutlineColor"),glowColor);
        _material.SetFloat(Shader.PropertyToID("_OutlineThickness"), _outlineThickness);
    }

   
}
