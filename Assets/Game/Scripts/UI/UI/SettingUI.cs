
using DG.Tweening;
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
        exitUIBtn.onClick.AddListener(() => UIScreen.HideUI<SettingUI>());
        exitGameBtn.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        UIScreen.HideUI<SettingUI>( false, () =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        });
       
    }
}
