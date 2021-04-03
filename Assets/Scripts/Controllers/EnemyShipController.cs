using Behaviours;
using Data;
using Events;
using pEventBus;
using Services;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Controllers
{
    public class EnemyShipController : BaseShipEntity, IEventReceiver<EnemyShipMoveEvent>, IEventReceiver<ShootLaserEnemyEvent>
    {
        [Inject] private IEnemyMovementService iMovementService;
        private float enemySpeed;
        private Vector2 enemyDirection;
        private SpaceInvadersEnemyMovementService enemyMovementService;
        private ShootingEntityBehavior shootingBehavior;

        protected override void Start()
        {
            base.Start();
            EventBus.Register(this);
            enemyMovementService = iMovementService as SpaceInvadersEnemyMovementService;
            shootingBehavior = GetComponent<ShootingEntityBehavior>();
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

        public void OnEvent(ShootLaserEnemyEvent e)
        {
            if (e.EnemyShipName != gameObject.name)
            {
                return;
            }
            
            shootingBehavior.ShootLaserProjectile(ConstValues.ShootingEntityType.ENEMY);
        }
    }
}

