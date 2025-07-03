
using UnityEngine;

[CreateAssetMenu(menuName = "Data/World/World Effect/WorldPoisonedEffect", fileName = "WorldPoisonedEffect")]
public class WorldPoisonedEffect : WorldEffect
{
    public int PoisonedDamage = 1;
    public int Level = 1;
    public int Rate = 1;
    [SerializeField] private Player player;
    public override void RegisterEnvent()
    {
        ObserverManager<GameEventType>.Attach(GameEventType.PlayCard, ApplyPoinsoned);
        ObserverManager<GameEventType>.Attach(GameEventType.GoDeep, UpLevel);
    }

    public override void UnRegisterEvent()
    {
        ObserverManager<GameEventType>.Detach(GameEventType.PlayCard, ApplyPoinsoned);
        ObserverManager<GameEventType>.Detach(GameEventType.GoDeep, UpLevel);
    }

    private void ApplyPoinsoned(object param)
    {
        if (player == null) player = FindObjectOfType<Player>();
        if (player == null) return;
        player.StatsSystem.TakeDamage(GetPoisonedDamage());
    }

    private int GetPoisonedDamage() => PoisonedDamage + Level * Rate;

    private void UpLevel(object param)
    {
        Level = (int)param;
    }

}
