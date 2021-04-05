using pEventBus;

namespace Events
{
    public struct ShowEndLevelPanelEvent : IEvent
    {
        public bool DidWinLevel { get; set; }
        public int TotalScore { get; set; }
    }
}
