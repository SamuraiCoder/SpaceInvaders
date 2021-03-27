using pEventBus;

namespace Data
{
    public struct EnemyBorderEvent : IEvent
    {
        public string EnemyTouchedBorderName { get; set; }
    }
}
