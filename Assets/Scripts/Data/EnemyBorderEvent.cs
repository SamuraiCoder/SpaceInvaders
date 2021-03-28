using pEventBus;

namespace Data
{
    public struct EnemyBorderEvent : IEvent
    {
        public string EnemyTouchedBorderName { get; set; }
        public bool EnemyTouchLeft { get; set; }
        public bool EnemyTouchRight { get; set; }
    }
}
