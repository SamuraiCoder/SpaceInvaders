using pEventBus;
using Events;
using Lean.Touch;
using UnityEngine;

namespace Controllers
{
    public class PlayerShipInteractor : MonoBehaviour
    {
        private enum PlayerDirection
        {
            GOING_LEFT,
            GOING_RIGHT,
            GOING_STATIC
        }
        
        private Vector2 directionVector;
        private Vector2 oldDirectionVector;
        private Vector2 directionPlayerShip;
        private bool isFingerSwipe;
        private PlayerDirection playerDirection;

        private void OnEnable()
        {
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            LeanTouch.OnFingerUp += OnFingerUp;
        }
        
        private void OnDisable()
        {
            LeanTouch.OnFingerTap += OnFingerTap;
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
            LeanTouch.OnFingerUp -= OnFingerUp;
        }

        private void OnFingerDown(LeanFinger finger)
        {
            playerDirection = PlayerDirection.GOING_STATIC;
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            directionVector = finger.ScreenDelta;
        }
        
        private void OnFingerUp(LeanFinger obj)
        {
            playerDirection = PlayerDirection.GOING_STATIC;
        }

        private void Update()
        {
            OnPlayerDirection();
            OnPlayerMovementInput();
        }

        private void OnPlayerDirection()
        {
            if (directionVector.x < -0.5f)
            {
                playerDirection = PlayerDirection.GOING_LEFT;
            }

            if (directionVector.x > 0.5f)
            {
                playerDirection = PlayerDirection.GOING_RIGHT;
            }

            if (Mathf.Approximately(directionVector.x, 0.0f))
            {
                playerDirection = PlayerDirection.GOING_STATIC;
            }
        }

        private void OnPlayerMovementInput()
        {
            var playerShipDirection = Vector2.zero;
            
            switch (playerDirection)
            {
                case PlayerDirection.GOING_LEFT:
                {
                    playerShipDirection += Vector2.left;
                    break;
                }
                case PlayerDirection.GOING_RIGHT:
                {
                    playerShipDirection += Vector2.right;
                    break;
                }
                case PlayerDirection.GOING_STATIC:
                {
                    playerShipDirection = Vector2.zero;
                    break;
                }
            }

            EventBus<PlayerShipMoveEvent>.Raise(new PlayerShipMoveEvent
            {
                Direction = playerShipDirection,
            });
        }
        
        private void OnFingerTap(LeanFinger obj)
        {
            if (!obj.Tap || obj.StartedOverGui)
            {
                return;
            }
            
            EventBus<ShootLaserPlayerEvent>.Raise(new ShootLaserPlayerEvent());
        }
    }
}
