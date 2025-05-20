
using Game.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InGameScreenUI : UIScreen
{
    [Header("Turn Button")]
    [SerializeField] private Button turnBtn;

    [SerializeField] private TextMeshProUGUI turnTxt;
    
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (turnBtn == null) turnBtn = transform.Find("Turn Button").GetComponent<Button>();
        if (turnTxt == null)
        {
            turnTxt = transform.Find("Turn Button").GetComponentInChildren<TextMeshProUGUI>();
            turnTxt.text = "End Turn";
        }
    }

    private void OnEnable()
    {
        ObserverManager<GameStateType>.Attach(GameStateType.PlayerTurn, param => OnPlayerTurn());
        turnBtn.onClick.AddListener(OnTurnBtnClick);
        turnBtn.interactable = false;

    }

    private void OnDisable()
    {
        turnBtn.onClick.RemoveAllListeners();
        ObserverManager<GameStateType>.DetachAll();
    }

    private void OnTurnBtnClick()
    {
        turnTxt.text = "Enemy Turn";
        turnBtn.interactable = false;
        GameManager.Instance.TakeTurn();
    }

    private void OnPlayerTurn()
    {
        turnTxt.text = "End Turn";
        turnBtn.interactable = true;
    }

}
