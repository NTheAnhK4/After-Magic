
using Game.UI;
using TMPro;
using UnityEngine;


public class LobbyScreenUI : UIScreen
{
    [SerializeField] private ButtonAnimBase settingBtn;
    [SerializeField] private TextMeshProUGUI coinTxt;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (settingBtn == null) settingBtn = transform.Find("Top/Setting").GetComponent<ButtonAnimBase>();
        if (coinTxt == null) coinTxt = transform.Find("Top/Top Left/Coin Infor").GetComponentInChildren<TextMeshProUGUI>();
        AddUIView<SettingUI>();
        AddUIView<WorldDescriptionUI>();
    }

    private async void OnSettingBtnClick() => await ShowUI<SettingUI>();

    private void OnEnable()
    {
        settingBtn.onClick += OnSettingBtnClick;

        int coinAmount = InventoryManager.Instance.GetAmountFromEquippedItems(ItemType.Coin);
        coinTxt.text = coinAmount.ToString();
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
