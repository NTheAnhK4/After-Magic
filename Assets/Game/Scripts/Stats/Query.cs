namespace BrokerChain
{
    public class Query
    {
        public readonly StatsType StatsType;
        public int Value;

        public Query(StatsType statsType, int value)
        {
            this.StatsType = statsType;
            this.Value = value;
        }
    }
}