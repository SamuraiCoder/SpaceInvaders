using pEventBus;
using UnityEngine;

namespace Events
{
    public struct EnemyShipMoveEvent : IEvent
    {
        public string EnemyShipName { get; set; }
        public Vector2 Direction { get; set; }
        public float Speed { get; set; }
    }
}

