using System;
using pEventBus;
using Events;
using Lean.Touch;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Controllers
{
    public class PlayerShipInteractor : MonoBehaviour
    {
        private Vector2 directionVector;
        private bool isFingerSwipe;

        private void OnEnable()
        {
            LeanTouch.OnFingerDown += OnFingerDown;
            LeanTouch.OnFingerSwipe += OnFingerSwipe;
            LeanTouch.OnFingerUp += OnFingerUp;
        }
        
        private void OnDisable()
        {
            LeanTouch.OnFingerDown -= OnFingerDown;
            LeanTouch.OnFingerSwipe -= OnFingerSwipe;
            LeanTouch.OnFingerUp -= OnFingerUp;
        }

        private void OnFingerDown(LeanFinger finger)
        {
            isFingerSwipe = true;
        }

        private void OnFingerSwipe(LeanFinger finger)
        {
            directionVector = finger.ScreenPosition;
        }
        
        private void OnFingerUp(LeanFinger obj)
        {
            isFingerSwipe = false;
        }

        private void Update()
        {
            if (!isFingerSwipe)
            {
                return;
            }
            
            Debug.Log(directionVector);
        }
        
        private void OnPlayerKeyboardMovementInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                //directionVector += Vector2.up;
            }
            
            if (Input.GetKey(KeyCode.S))
            {
                directionVector += Vector2.down;
            }
            
            if (Input.GetKey(KeyCode.A))
            {
                directionVector += Vector2.left;
            }
            
            if (Input.GetKey(KeyCode.D))
            {
                directionVector += Vector2.right;
            }

            if (directionVector == Vector2.zero)
            {
                return;
            }
            
            EventBus<PlayerShipMoveEvent>.Raise(new PlayerShipMoveEvent
            {
                Direction = directionVector,
            });
        }
    }
}
