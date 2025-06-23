using System;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class ButtonAnimBase : ComponentBehavior
{
    [SerializeField] private Button btn;
    public Action onClick = delegate{};
    public Action beforeClick = delegate {  };

    public bool Interacable
    {
        set => btn.interactable = value;
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (btn == null) btn = GetComponent<Button>();
    }

    
    private async void OnButtonClick()
    {
        beforeClick?.Invoke();
        await RunEffect();
        onClick?.Invoke();
    }

    protected abstract UniTask RunEffect();
    private void OnEnable()
    {
        btn.onClick.AddListener(OnButtonClick);
    }

    private void OnDisable()
    {
        btn.onClick.RemoveListener(OnButtonClick);
    }
}
