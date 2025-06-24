

public class CardComponent : ComponentBehavior
{
    protected Card card;
    public override void LoadComponent()
    {
        base.LoadComponent();
        if (card == null) card = GetComponent<Card>();
    }

   
}
