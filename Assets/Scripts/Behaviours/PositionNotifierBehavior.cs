using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Behaviours
{
    public class PositionNotifierBehavior : MonoBehaviour
    {
        [Inject] private IPositionService gameEntitiesPositionService;
        
        private Vector2 lastEntityPosition;
        
        private void Update()
        {
            if (lastEntityPosition == (Vector2)transform.position)
            {
                return;
            }

            lastEntityPosition = transform.position;
            
            RegisterEntityPosition();
        }

        private void RegisterEntityPosition()
        {
            gameEntitiesPositionService.RegisterEntityPosition(gameObject.name, transform.position);
        }

        private void OnDestroy()
        {
            gameEntitiesPositionService.UnRegisterEntityPosition(gameObject.name);
        }
    }
}
