using pEventBus;

namespace Events
{
    public struct PlayerScoreAmountEvent : IEvent
    {
        public int Score { get; set; }
    }
}
