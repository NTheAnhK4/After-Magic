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
public enum DungeonRoomType
{
    Battle,
    Campfire,
    Door,
    Empty,
    Entry,
    Mystery,
    Shop
}