using pEventBus;

namespace Events
{
    public struct EnemyDestroyedScoreEvent : IEvent
    {
        public int ScorePerDeath { get; set; }
    }
}
