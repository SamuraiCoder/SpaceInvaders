using pEventBus;

namespace Events
{
    public struct EndLevelConditionEvent : IEvent
    {
        public bool WinLevel { get; set; }
    }
}
