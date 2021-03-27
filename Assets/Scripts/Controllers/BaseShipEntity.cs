using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public abstract class BaseShipEntity : MonoBehaviour
    {
        [Inject] private IPositionService gameEntitiesPositionService;
        
        protected float speed;
        protected Vector2 direction;
        protected bool touchingRightLimit;
        protected bool touchingLeftLimit;

        private Vector2 screenLimits;

        protected virtual void Start()
        {
            screenLimits = new Vector2(Screen.width, Screen.height);
        }

        protected virtual void Update()
        {
            MoveEntity();
            OnCheckScreenLimits();
        }
        
        private void MoveEntity()
        {
            transform.Translate(direction * speed * Time.deltaTime);
        }

        private void OnCheckScreenLimits()
        {
            var shipEntityWorldPos = gameEntitiesPositionService.GetEntityPosition(ConstValues.PLAYER_NAME);

            var maxPosShip = shipEntityWorldPos.x;
            var halfShipWidth = ConstValues.PLAYER_WIDTH / 2;

            touchingLeftLimit = maxPosShip - halfShipWidth <= 0;

            touchingRightLimit = maxPosShip + halfShipWidth >= screenLimits.x;
        }
    }
}
