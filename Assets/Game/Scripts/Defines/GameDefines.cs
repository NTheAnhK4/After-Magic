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

public enum CardActionType
{
    IncreaseManaCost,
    DecreaseManaCost
}

public enum GameEventType
{
    Win, 
    Lose,
    ChanegCoin,
    PlayCard,
    GoDeep,
    FinishDistributeCard
}

public enum CardType
{
    Attack,
    Skill,
    Power,
    Status
}
public enum DungeonRoomType
{
    Battle = 0,
    Campfire = 1,
    Door = 2,
    Empty = 3,
    Entry = 4,
    Mystery = 5,
    Shop = 6
}


namespace BrokerChain
{
    public enum StatsType
    {
        Damage,
        Defense,
        ExtraTakenDamage
    }

    

    public enum OperatorType
    {
        Add,
        Multiply
    }
}
