
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class InGameInitializer : IGameInitializer
{
    public InGameManager InGameManagerPrefab;
    public InGameUICtrl InGameUI;
    public WorldEffectCtrl WorldEffectCtrlPrefab;
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

        if (WorldEffectCtrlPrefab == null)
        {
            Debug.LogWarning("World Effect Ctrl is null");
            return;
        }

        WorldEffectCtrl worldEffectCtrl = Instantiate(WorldEffectCtrlPrefab);
        
        WorldData worldData = GameManager.Instance.GetCurrentWorldData();
        
        if(worldData != null) worldEffectCtrl.Init(worldData.WorldEffect);
        await UniTask.Delay(50, DelayType.UnscaledDeltaTime);

    }
}
