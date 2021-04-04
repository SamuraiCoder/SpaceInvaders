using UnityEngine;
using pEventBus;

namespace Events
{
    public struct SpawnPlayerEvent : IEvent
    {
        public Vector2 SpawnPosition { get; set; }
    }
}
