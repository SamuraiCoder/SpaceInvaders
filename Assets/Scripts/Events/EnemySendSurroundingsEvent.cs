using Data;
using pEventBus;

namespace Events
{
    public struct EnemySendSurroundingsEvent : IEvent
    {
        public string EnemyShipName { get; set; }
        public EnemyData EnemyDataInfo { get; set; }
    }
}
