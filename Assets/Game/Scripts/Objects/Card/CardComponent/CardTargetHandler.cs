
using UnityEngine;

public class CardTargetHandler : CardComponent
{
    public Selectable CurrentTarget { get; private set; }

    public void ResetTarget() => CurrentTarget = null;

    private void OnEnable()
    {
        ResetTarget();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var target = other.GetComponent<Selectable>();
        if (!IsValidTarget(target)) return;
        CurrentTarget = target;
        CurrentTarget.SelectObject();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        
        var target = other.GetComponent<Selectable>();
        if (!IsValidTarget(target)) return;
        target.DeselectObject();
        if(target == CurrentTarget) ResetTarget();
    }

    private bool IsValidTarget(Selectable target)
    {
        if (target == null) return false;
        var strategy = card.CardDataCtrl.CardStrategy;
        return strategy.AppliesToAlly ? target.CompareTag("Player") : target.CompareTag("Enemy");
    }
}
