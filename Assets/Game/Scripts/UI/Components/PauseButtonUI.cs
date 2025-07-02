

using Game.UI;
using UnityEngine;
using UnityEngine.UI;

public class PauseButtonUI : ComponentBehaviour
{
    [SerializeField] private Button _button;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (_button == null) _button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        _button.onClick.AddListener(OnPauseBtnClick);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveListener(OnPauseBtnClick);
    }

    private async void OnPauseBtnClick()
    {
        if (UIScreen.Instance == null)
        {
            Debug.LogWarning("UIScreen is null");
            return;
        }

        await UIScreen.Instance.ShowUI<PauseUI>();
    }
}
