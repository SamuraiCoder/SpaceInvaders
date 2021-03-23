using Events;
using pEventBus;
using UnityEngine;

namespace Controllers
{
    public class PlayerShipController : Entity, IEventReceiver<PlayerShipMoveEvent>
    {
        [SerializeField] private float playerSpeed;

        private Vector2 playerDirection;

        private void Start()
        {
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        protected override void Update()
        {
            direction = playerDirection;
            speed = playerSpeed;
            
            base.Update();
        }

        public void OnEvent(PlayerShipMoveEvent e)
        {
            playerDirection = e.Direction;
        }
    }
}
