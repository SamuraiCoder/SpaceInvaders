using pEventBus;
using UnityEngine;

namespace Events
{
    public struct SpawnShieldEvent : IEvent
    {
        public Vector2 SpawnPosition { get; set; }
        public int ShieldHitsPerBlock { get; set; }
    }
}
