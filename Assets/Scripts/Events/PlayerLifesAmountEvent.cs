using pEventBus;

namespace Events
{
    public struct PlayerLifesAmountEvent : IEvent
    {
        public int Lifes { get; set; }
    }
}
