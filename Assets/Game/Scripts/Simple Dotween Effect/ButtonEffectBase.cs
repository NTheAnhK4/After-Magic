using System;
using Cysharp.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

public abstract class ButtonEffectBase : ComponentBehavior
{
    [SerializeField] private Button btn;
    private Action onClick = delegate{};

    public bool Interacable
    {
        set => btn.interactable = value;
    }
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (btn == null) btn = GetComponent<Button>();
    }

    public void Init(Action onClickAction) => onClick = onClickAction;
    private async void OnButtonClick()
    {
        
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
        btn.onClick.RemoveAllListeners();
    }
}
