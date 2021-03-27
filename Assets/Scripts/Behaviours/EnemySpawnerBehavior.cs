using Events;
using pEventBus;
using UnityEngine;

namespace Behaviours
{
    public class EnemySpawnerBehavior : MonoBehaviour, IEventReceiver<SpawnEnemyEvent>
    {
        [SerializeField] private GameObject enemyPrefab;

        private void Start()
        {
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(SpawnEnemyEvent e)
        {
            OnSpawnEnemyShip(e);
        }

        private void OnSpawnEnemyShip(SpawnEnemyEvent spawnEnemyEvent)
        {
            var go = Instantiate(enemyPrefab, spawnEnemyEvent.SpawnPosition, transform.rotation, gameObject.transform);
            go.name = spawnEnemyEvent.EnemyName;
        }
    }
}
