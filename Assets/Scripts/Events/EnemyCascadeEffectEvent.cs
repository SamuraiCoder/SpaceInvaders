using pEventBus;

namespace Events
{
    public struct EnemyCascadeEffectEvent : IEvent
    {
        public string EnemyShipName { get; set; }
    }
}
