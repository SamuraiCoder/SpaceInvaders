using UnityEngine;

namespace Behaviours
{
    public class ShootingEntityBehavior : MonoBehaviour
    {
        [SerializeField] private GameObject laserProjectilePrefab;
        [SerializeField] private GameObject shootingEntityPosition;
        
        public void ShootLaserProjectile(ConstValues.ShootingEntityType shootingEntityType)
        {
            OnShootingEntity(shootingEntityType);
        }

        private void OnShootingEntity(ConstValues.ShootingEntityType shootingEntityType)
        {
            var currentTransform = gameObject.transform;
            var laser = Instantiate(laserProjectilePrefab, currentTransform.position, currentTransform.rotation, shootingEntityPosition.transform);
            var laserBehavior = laser.GetComponent<LaserBehavior>();
            laserBehavior.SetShootingEntityType(shootingEntityType);
        }
    }
}
