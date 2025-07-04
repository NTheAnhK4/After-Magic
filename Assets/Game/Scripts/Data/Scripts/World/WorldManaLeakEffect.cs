
using UnityEngine;
[CreateAssetMenu(menuName = "Data/World/World Effect/WorldManaLeakEffect", fileName = "WorldManaLeakEffect")]
public class WorldManaLeakEffect : WorldEffect
{
    
    public override void RegisterEnvent()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.FinishDistributeCard, NotifyIncreaseManaCost);
    }

    public override void UnRegisterEvent()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.FinishDistributeCard, NotifyIncreaseManaCost);
    }

    private void NotifyIncreaseManaCost(object param)
    {
        ObserverManager<CardActionType>.Notify(CardActionType.IncreaseManaCost,1);
    }
    
}
