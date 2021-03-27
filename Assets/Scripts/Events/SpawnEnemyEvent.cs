using pEventBus;
using UnityEngine;

namespace Events
{
    public struct SpawnEnemyEvent : IEvent
    {
        public Vector2 SpawnPosition { get; set; }
        public string EnemyName { get; set; }
        public string SpriteString { get; set; }

        public ConstValues.ColorEnemyPool Color { get; set; }

    }
}
