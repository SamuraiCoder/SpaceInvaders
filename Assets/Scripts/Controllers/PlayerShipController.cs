﻿using Events;
using pEventBus;
using UnityEngine;

namespace Controllers
{
    public class PlayerShipController : BaseShipEntity, IEventReceiver<PlayerShipMoveEvent>
    {
        [SerializeField] private float playerSpeed;

        private Vector2 playerDirection;

        protected override void Start()
        {
            base.Start();
            
            EventBus.Register(this);
            
            speed = playerSpeed;
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        protected override void Update()
        {
            base.Update();

            OnPlayerInputReceived();
        }

        private void OnPlayerInputReceived()
        {
            if (touchingLeftLimit && playerDirection == Vector2.left)
            {
                OnPlayerDirection(Vector2.zero, 0.0f);
                return;
            }
            
            if (touchingRightLimit && playerDirection == Vector2.right)
            {
                OnPlayerDirection(Vector2.zero, 0.0f);
                return;
            }
            
            if (touchingLeftLimit && direction == Vector2.left)
            {
                OnPlayerDirection(Vector2.zero, 0.0f);
                return;
            }
            
            if (touchingRightLimit && direction == Vector2.right)
            {
                OnPlayerDirection(Vector2.zero, 0.0f);
                return;
            }

            OnPlayerDirection(playerDirection, playerSpeed);
        }

        private void OnPlayerDirection(Vector2 dir, float speed)
        {
            direction = dir;
        }

        public void OnEvent(PlayerShipMoveEvent e)
        {
            playerDirection = e.Direction;
        }
    }
}
