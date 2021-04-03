using Data;
using Events;
using pEventBus;
using Services;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class EnemyShipController : BaseShipEntity, IEventReceiver<EnemyShipMoveEvent>
    {
        [Inject] private IEnemyMovementService iMovementService;
        private float enemySpeed;
        private Vector2 enemyDirection;
        private SpaceInvadersEnemyMovementService enemyMovementService;

        protected override void Start()
        {
            base.Start();
            EventBus.Register(this);
            enemyMovementService = iMovementService as SpaceInvadersEnemyMovementService;
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
                //Note: We send just the first event that touched screen until we release it again
                if (!enemyMovementService.TouchedEvent)
                {
                    EventBus<EnemyBorderEvent>.Raise(new EnemyBorderEvent
                    {
                        EnemyTouchedBorderName = gameObject.name,
                        EnemyTouchLeft = touchingLeftLimit,
                        EnemyTouchRight = touchingRightLimit
                    });
                    
                    //Debug.Log($"Enemy {gameObject.name} touched!");
                }
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

