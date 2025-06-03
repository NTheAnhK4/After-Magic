

public class CardComponent : ComponentBehavior
{
    protected Card card;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (card == null) card = GetComponent<Card>();
    }

   
}
