
using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeSetter : ComponentBehaviour
{
    [SerializeField] private Material _material;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (_material == null) _material = GetComponent<Image>().material;
    }
    void Update()
    {
        _material.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
    
}
