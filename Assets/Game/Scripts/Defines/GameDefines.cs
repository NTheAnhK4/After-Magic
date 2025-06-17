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
    Win, Lose,GainCoin
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

public enum ItemType
{
    Coin,
}

namespace BrokerChain
{
    public enum StatsType
    {
        Damage,
        Defense,
       
    }

    public enum ExpireTiming
    {
        EndOfThisTurn,
        StartOfThisTurn
    }

    public enum OperatorType
    {
        Add,
        Multiply
    }
}
