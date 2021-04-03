using pEventBus;

namespace Events
{
    public struct ShootLaserEnemyEvent : IEvent
    {
        public string EnemyShipName { get; set; }
    }
}
