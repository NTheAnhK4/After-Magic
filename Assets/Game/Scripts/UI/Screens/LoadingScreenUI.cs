
using DG.Tweening;
using SaveGame;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : ComponentBehavior
{
    public Slider slider;
    [SerializeField] private ButtonAnimBase playBtn;
    
    private bool isLoaded;
    public bool IsDataLoaded;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (slider == null) slider = GetComponentInChildren<Slider>();
        if (playBtn == null) playBtn = GetComponentInChildren<ButtonAnimBase>();
       
        slider.value = 0;
        playBtn.gameObject.SetActive(false);
        isLoaded = false;
    }

  
   


    private void OnEnable()
    {
        playBtn.onClick += OnPlayGame;
        slider.onValueChanged.AddListener(OnSliderValueChange);
    }

    private void OnDisable()
    {
        playBtn.onClick -= OnPlayGame;
        slider.onValueChanged.RemoveListener(OnSliderValueChange);
    }

    private void OnSliderValueChange(float value)
    {
        if(isLoaded) return;
        if (slider.value >= .99)
        {
            isLoaded = true;
            ShowPlayBtn();
            slider.gameObject.SetActive(false);
        }
    }



   

    private async void ShowPlayBtn()
    {
        playBtn.transform.localScale = Vector3.zero;
        playBtn.Interacable = false;
        playBtn.gameObject.SetActive(true);

        await playBtn.transform.DOScale(1f, .5f).SetUpdate(true).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        playBtn.Interacable = true;
    }

   

    private void OnPlayGame()
    {
        if(IsDataLoaded) SceneLoader.Instance.LoadScene(SaveLoadSystem.Instance.GameData.CurrentLevelName);
        
        else SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
    }
}
