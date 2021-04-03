using pEventBus;

namespace Events
{
    public struct EnemyNotifySurroundingsEvent : IEvent
    {
        public string EnemyShipName { get; set; }
    }
}
