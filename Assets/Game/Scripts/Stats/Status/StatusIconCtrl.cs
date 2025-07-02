using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusIconCtrl : ComponentBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (image == null) image = GetComponentInChildren<Image>();
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public void Init(Sprite icon = null, string textValue = null)
    {
        if(icon != null) image.sprite = icon;
        if(textValue != null) text.text = textValue;
    }
}
