
using Game.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : UIView
{
    [SerializeField] private Button exitGameBtn;
    [SerializeField] private Button exitUIBtn;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (exitUIBtn == null) exitUIBtn = transform.Find("Exit").GetComponent<Button>();
        if (exitGameBtn == null) exitGameBtn = transform.Find("Buttons").Find("ExitGame").GetComponent<Button>();
    }

    private void Start()
    {
        exitUIBtn.onClick.AddListener(OnExitBtnClick);
        exitGameBtn.onClick.AddListener(ExitGame);
    }

    private async void OnExitBtnClick() => await UIScreen.HideUI<SettingUI>();

    private async void ExitGame()
    {
        await UIScreen.HideUI<SettingUI>( false, () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
       
    }
}
