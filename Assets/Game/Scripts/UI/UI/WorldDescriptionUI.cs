using System;

using Game.Defines;
using Game.UI;
using TMPro;

using UnityEngine;
using UnityEngine.UI;

public class WorldDescriptionUI : UIView
{
    [SerializeField] private Button exitBtn;
    [SerializeField] private ButtonAnimBase playBtn;
    [SerializeField] private Image worldImg;
    [SerializeField] private TextMeshProUGUI worldName;
    [SerializeField] private TextMeshProUGUI worldDescription;
    public override void LoadComponent()
    {
        base.LoadComponent();
        ShowAnimation = ViewAnimationType.DipToBlack;
        HideAnimation = ViewAnimationType.DipToBlack;
        if (exitBtn == null) exitBtn = transform.Find("Exit").GetComponent<Button>();
        if (playBtn == null) playBtn = transform.Find("Play").GetComponent<ButtonAnimBase>();
        if (worldImg == null) worldImg = transform.Find("WorldMask/WorldImg").GetComponent<Image>();
        if (worldName == null) worldName = transform.Find("World Name").GetComponent<TextMeshProUGUI>();
        if (worldDescription == null) worldDescription = transform.Find("Description/View/Content").GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        exitBtn.onClick.AddListener(OnExitBtnClick);
        playBtn.onClick += OnPlayBtnClick;
    }

    private void OnDisable()
    {
        exitBtn.onClick.RemoveListener(OnExitBtnClick);
        playBtn.onClick -= OnPlayBtnClick;
    }

    private async void OnExitBtnClick() => await UIScreen.HideUI<WorldDescriptionUI>();
    private void OnPlayBtnClick() => SceneLoader.Instance.LoadScene(GameConstants.DungeonScene);
    public override void Show()
    {
        Init();
        base.Show();
    }

    private void Init()
    {
        WorldData worldData = GameManager.Instance.GetWorldData();
        if (worldData == null) return;
        
        if(worldData.WorldSprite != null) worldImg.sprite = worldData.WorldSprite;
        if(!String.IsNullOrEmpty(worldData.WorldName)) worldName.text = worldData.WorldName;
        if (!String.IsNullOrEmpty(worldData.Description)) worldDescription.text = worldData.Description;
    }
}
