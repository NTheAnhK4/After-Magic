namespace Develop
{
    public enum StatsType
    {
        Attack,
        Defense
    }

    public class Stats
    {
        private readonly StatsMediator mediator;
        private readonly BaseStats baseStats;

        public int Attack
        {
            get
            {
                var q = new Query(StatsType.Attack, baseStats.attack);
                mediator.PerformQery(this, q);
                return q.Value;
            }
        }

        public int Defense
        {
            get
            {
                var q = new Query(StatsType.Defense, baseStats.defense);
                mediator.PerformQery(this, q);
                return q.Value;
            }
        }

        public Stats(StatsMediator mediator, BaseStats baseStats)
        {
            this.mediator = mediator;
            this.baseStats = baseStats; 
        }
    }
    
}