using pEventBus;
using UnityEngine;

namespace Events
{
    public struct SpawnEnemyEvent : IEvent
    {
        public Vector2 SpawnPosition { get; set; }
        public string EnemyName { get; set; }
    }
}
