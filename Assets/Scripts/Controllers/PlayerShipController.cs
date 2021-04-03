using Behaviours;
using Events;
using pEventBus;
using UnityEngine;

namespace Controllers
{
    public class PlayerShipController : BaseShipEntity, IEventReceiver<PlayerShipMoveEvent>, IEventReceiver<ShootLaserPlayerEvent>
    {
        [SerializeField] private float playerSpeed;

        private Vector2 playerDirection;
        private ShootingEntityBehavior shootingBehavior;
        private int laserProjectileLayer;

        protected override void Start()
        {
            base.Start();
            EventBus.Register(this);
            speed = playerSpeed;
            shootingBehavior = GetComponent<ShootingEntityBehavior>();
            laserProjectileLayer = LayerMask.NameToLayer(ConstValues.LASER_ENEMY_LAYER);
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
                OnPlayerDirection(Vector2.zero);
                return;
            }
            
            if (touchingRightLimit && playerDirection == Vector2.right)
            {
                OnPlayerDirection(Vector2.zero);
                return;
            }
            
            if (touchingLeftLimit && direction == Vector2.left)
            {
                OnPlayerDirection(Vector2.zero);
                return;
            }
            
            if (touchingRightLimit && direction == Vector2.right)
            {
                OnPlayerDirection(Vector2.zero);
                return;
            }

            OnPlayerDirection(playerDirection);
        }

        private void OnPlayerDirection(Vector2 dir)
        {
            direction = dir;
        }

        public void OnEvent(PlayerShipMoveEvent e)
        {
            playerDirection = e.Direction;
        }

        public void OnEvent(ShootLaserPlayerEvent e)
        {
            shootingBehavior.ShootLaserProjectile(ConstValues.ShootingEntityType.PLAYER);
        }
        
        private void OnTriggerEnter2D(Collider2D obj)
        {
            if (obj.gameObject.layer != laserProjectileLayer)
            {
                return;
            }
            
            //Destroy laser
            Destroy(obj.gameObject);
            
            OnDestroyPlayerShip();
            
            EventBus<PlayerShipDestroyedEvent>.Raise(new PlayerShipDestroyedEvent());
        }
        
        private void OnDestroyPlayerShip()
        {
            LeanTween.rotateAround(gameObject, Vector3.forward, 360, 0.25f).setOnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}
