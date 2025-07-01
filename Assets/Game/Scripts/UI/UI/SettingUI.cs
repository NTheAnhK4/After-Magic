
using AudioSystem;
using Game.UI;

using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SettingUI : UIView
{
    [SerializeField] private ButtonAnimBase exitGameBtn;
    [SerializeField] private Button exitUIBtn;
    [SerializeField] private Slider soundSlider;
    [SerializeField] private Slider musicSlider;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (exitUIBtn == null) exitUIBtn = transform.Find("Exit").GetComponent<Button>();
        if (exitGameBtn == null) exitGameBtn = transform.Find("Buttons").Find("ExitGame").GetComponent<ButtonAnimBase>();

        Transform SoundAndMusicHolder = transform.Find("Sound and Music");
        if (musicSlider == null) musicSlider = SoundAndMusicHolder.Find("Music").GetComponentInChildren<Slider>();
        if (soundSlider == null) soundSlider = SoundAndMusicHolder.Find("Sound").GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        exitUIBtn.onClick.AddListener(OnExitBtnClick);
        exitGameBtn.onClick += ExitGame;
        musicSlider.onValueChanged.AddListener(OnMusicValueChange);
        soundSlider.onValueChanged.AddListener(OnSoundValueChange);
    }

    private void OnDestroy()
    {
        exitUIBtn.onClick.RemoveAllListeners();
        exitGameBtn.onClick -= ExitGame;
        musicSlider.onValueChanged.RemoveAllListeners();
        soundSlider.onValueChanged.RemoveAllListeners();
    }

    public override void Show()
    {
        ObserverManager<SoundActionType>.Notify(SoundActionType.PauseAll);
        if (MusicManager.Instance != null) musicSlider.value = MusicManager.Instance.Volume;
        if (SoundManager.Instance != null) soundSlider.value = SoundManager.Instance.SoundRate;
        base.Show();
    }

    private void OnMusicValueChange(float value)
    {
        if (MusicManager.Instance != null) MusicManager.Instance.Volume = value;
    }

    private void OnSoundValueChange(float value)
    {
        if (SoundManager.Instance != null) SoundManager.Instance.SoundRate = value;
    }


    private async void OnExitBtnClick()
    {
        await UIScreen.HideUI<SettingUI>();
        ObserverManager<SoundActionType>.Notify(SoundActionType.UnPauseAll);
    }

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
