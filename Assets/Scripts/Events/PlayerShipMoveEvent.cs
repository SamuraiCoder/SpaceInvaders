using pEventBus;
using UnityEngine;

namespace Events
{
    public struct PlayerShipMoveEvent : IEvent
    {
        public Vector2 Direction { get; set; }
    }
}

