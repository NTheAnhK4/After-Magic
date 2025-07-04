
using Game.UI;

using UnityEngine;


public class LobbyScreenUI : UIScreen
{
    [SerializeField] private ButtonAnimBase settingBtn;
    
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Top/Setting").GetComponent<ButtonAnimBase>();
       
        AddUIView<SettingUI>();
        AddUIView<WorldDescriptionUI>();
    }

    private async void OnSettingBtnClick() => await ShowUI<SettingUI>();

    private void OnEnable()
    {
        settingBtn.onClick += OnSettingBtnClick;
    }

    private void Start()
    {
        MusicManager.Instance.PlayMusic(MusicType.Lobby);
    }

    private void OnDisable()
    {
        settingBtn.onClick -= OnSettingBtnClick;
        
    }
}
