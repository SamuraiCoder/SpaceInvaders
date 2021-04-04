using pEventBus;

namespace Events
{
    public struct PauseEvent : IEvent
    {
        public bool Pause { get; set; }
    }
}
