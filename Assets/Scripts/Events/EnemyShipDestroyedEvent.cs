using pEventBus;

namespace Events
{
    public struct EnemyShipDestroyedEvent : IEvent
    {
        public string EnemyShipName { get; set; }
    }
}
