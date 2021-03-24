﻿using System;
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
            GOING_RIGHT
        }
        
        private Vector2 directionVector;
        private Vector2 directionPlayerShip;
        private bool isFingerSwipe;
        private PlayerDirection playerDirection;

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerUpdate += OnFingerUpdate;
            LeanTouch.OnFingerUp += OnFingerUp;
        }
        
        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerUpdate -= OnFingerUpdate;
            LeanTouch.OnFingerUp -= OnFingerUp;
        }

        private void OnFingerDown(LeanFinger finger)
        {
            isFingerSwipe = true;
        }

        private void OnFingerUpdate(LeanFinger finger)
        {
            directionVector = finger.ScreenDelta;
        }
        
        private void OnFingerUp(LeanFinger obj)
        {
            isFingerSwipe = false;
        }

        private void Update()
        {
            if (!isFingerSwipe || Mathf.Approximately(directionVector.x, 0.0f))
            {
                return;
            }

            OnPlayerDirection();
            
        }

        private void OnPlayerDirection()
        {
            //Debug.Log(directionVector.x < 0 ? $"SwipeLeft {directionVector.x}" : $"SwipeRight {directionVector.x}");
            if (directionVector.x < -0.1f)
            {
                playerDirection = PlayerDirection.GOING_LEFT;
            }

            if (directionVector.x > 0.1f)
            {
                playerDirection = PlayerDirection.GOING_RIGHT;
            }
            
            OnPlayerMovementInput();
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
            }

            EventBus<PlayerShipMoveEvent>.Raise(new PlayerShipMoveEvent
            {
                Direction = playerShipDirection,
            });
        }
    }
}
