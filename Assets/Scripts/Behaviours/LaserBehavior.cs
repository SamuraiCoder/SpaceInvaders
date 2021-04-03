using UnityEngine;

namespace Behaviours
{
    public class LaserBehavior : MonoBehaviour
    {
        [SerializeField] private float laserProjectileForce;

        private float laserProjectileTimer;
        private float laserProjectileCooldown;
        private Rigidbody2D laserRigidBody;
        private ConstValues.ShootingEntityType shootingEntityType;
        private Vector2 laserDirection;

        void Awake () 
        {
            laserRigidBody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            laserDirection = shootingEntityType == ConstValues.ShootingEntityType.ENEMY ? Vector2.down : Vector2.up;
            laserProjectileCooldown = shootingEntityType == ConstValues.ShootingEntityType.ENEMY
                ? ConstValues.LASER_COOLING_ENEMY
                : ConstValues.LASER_COOLING_PLAYER;
        }

        void LateUpdate() 
        {
            laserProjectileTimer += Time.deltaTime;
 
            if(laserProjectileTimer < laserProjectileCooldown)
            {
                OnAddForceProjectile();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnAddForceProjectile()
        {
            switch (shootingEntityType)
            {
                case ConstValues.ShootingEntityType.PLAYER:
                {
                    laserRigidBody.AddForce(laserDirection * laserProjectileForce, ForceMode2D.Impulse);
                    break;
                }
                case ConstValues.ShootingEntityType.ENEMY:
                {
                    laserRigidBody.AddForce(laserDirection * laserProjectileForce, ForceMode2D.Force);
                    break;
                }
            }
        }

        public void SetShootingEntityType(ConstValues.ShootingEntityType type) => shootingEntityType = type;
    }
}

