
using TMPro;
using UnityEngine;

public class CoinUICtrl : ComponentBehaviour
{
    [SerializeField] private TextMeshProUGUI coinTxt;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (coinTxt == null) coinTxt = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        OnCoinValueChange();
        ObserverManager<GameEventType>.Attach(GameEventType.ChanegCoin, OnCoinValueChange);
    }

    private void OnDisable()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.ChanegCoin, OnCoinValueChange);
    }

    private void OnCoinValueChange(object param = null)
    {
        int coinAmount = InventoryManager.Instance.GetAmountFromLoot(ItemType.Coin) + InventoryManager.Instance.GetAmountFromEquippedItems(ItemType.Coin);
        coinTxt.text = coinAmount.ToString();
    }
}
