using Data;
using Events;
using pEventBus;
using UnityEngine;

namespace Controllers
{
    public class EnemyShipController : BaseShipEntity, IEventReceiver<EnemyShipMoveEvent>
    {
        private float enemySpeed;
        private Vector2 enemyDirection;

        protected override void Start()
        {
            base.Start();
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        protected override void Update()
        {
            base.Update();

            OnEnemyAIInputReceived();
        }

        private void OnEnemyAIInputReceived()
        {
            if (touchingLeftLimit || touchingRightLimit)
            {
              //Notify Move service a ship has touched a border
              EventBus<EnemyBorderEvent>.Raise(new EnemyBorderEvent
              {
                  EnemyTouchedBorderName = gameObject.name,
                  EnemyTouchLeft = touchingLeftLimit,
                  EnemyTouchRight = touchingRightLimit
              });
            }
            
            direction = enemyDirection;
            speed = enemySpeed;
        }

        public void OnEvent(EnemyShipMoveEvent e)
        {
            if (e.EnemyShipName != gameObject.name)
            {
                return;
            }
            
            enemyDirection = e.Direction;
            enemySpeed = e.Speed;
        }
    }
}

