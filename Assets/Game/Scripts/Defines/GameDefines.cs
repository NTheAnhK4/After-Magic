public enum GameStateType
{
    DistributeCard,
    PlayerTurn,
    CollectingCard,
    EnemyTurn,
    UsingCard
}

public enum EnemyActionType
{
    Attack,
    Defense,
    Buff,
    Debuff
}

public enum CardEventType
{
    PlayerTarget,
    EnemyTarget,
    DrawPileCountChange,
    DiscardPileCountChange,
    DepleteCardsCountChange
}

public enum GameEventType
{
    Win, Lose
}
