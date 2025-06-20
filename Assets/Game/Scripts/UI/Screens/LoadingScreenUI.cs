
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreenUI : ComponentBehavior
{
    [SerializeField] private Slider slider;
    [SerializeField] private ButtonEffectBase playBtn;
    private bool isLoaded;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (slider == null) slider = GetComponentInChildren<Slider>();
        if (playBtn == null) playBtn = GetComponentInChildren<ButtonEffectBase>();
        playBtn.Init(OnPlayGame);
        slider.value = 0;
        playBtn.gameObject.SetActive(false);
        isLoaded = false;
    }

   

    private void Update()
    {
        if (isLoaded) return;
        slider.value = Mathf.Min(slider.value + Time.deltaTime, 1);
        if (slider.value >= .99)
        {
            isLoaded = true;
            slider.gameObject.SetActive(false);
            
            ShowPlayBtn();
           
        }
    }

    private async void ShowPlayBtn()
    {
        playBtn.transform.localScale = Vector3.zero;
        playBtn.Interacable = false;
        playBtn.gameObject.SetActive(true);

        await playBtn.transform.DOScale(1f, 1).SetUpdate(true).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        playBtn.Interacable = true;
    }

   

    private void OnPlayGame()
    {
        SceneLoader.Instance.LoadScene(GameConstants.LobbyScene);
    }
}
