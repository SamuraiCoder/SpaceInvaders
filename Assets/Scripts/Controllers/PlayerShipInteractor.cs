using pEventBus;
using Events;
using UnityEngine;

namespace Controllers
{
    public class PlayerShipInteractor : MonoBehaviour
    {
        private Vector2 directionVector;
        
        private void Update()
        {
            OnPlayerKeyboardMovementInput();
        }
        
        private void OnPlayerKeyboardMovementInput()
        {
            if (Input.GetKey(KeyCode.W))
            {
                directionVector += Vector2.up;
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
