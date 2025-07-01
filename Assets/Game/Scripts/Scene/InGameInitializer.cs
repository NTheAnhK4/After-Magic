
using Cysharp.Threading.Tasks;
using UnityEngine;

public class InGameInitializer : IGameInitializer
{
    public InGameManager InGameManagerPrefab;
    public InGameUICtrl InGameUI;
    public override async UniTask Init()
    {
        if (InGameManagerPrefab == null)
        {
            Debug.LogWarning("InGameManagerPrefab is null");
            return;
        }

        InGameManager inGameManager = Instantiate(InGameManagerPrefab);
        inGameManager.PlayGame();
        await UniTask.Delay(100, DelayType.UnscaledDeltaTime);
        PlayerPartyManager.Instance.InitData();
        if (InGameUI == null)
        {
            Debug.LogWarning("In Game UI is null");
            return;
        }

        InGameUICtrl inGameUI = Instantiate(InGameUI);
        inGameUI.InitData();
        
    }
}
