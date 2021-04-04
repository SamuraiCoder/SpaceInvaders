using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
    public class ShipSpawnerBehavior : MonoBehaviour, IEventReceiver<SpawnEnemyEvent>, IEventReceiver<SpawnPlayerEvent>
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject playerPrefab;

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
            go.GetComponent<Image>().sprite = Resources.Load<Sprite>(spawnEnemyEvent.SpriteString);
        }

        public void OnEvent(SpawnPlayerEvent e)
        {
            OnSpawnPlayer(e);
        }

        private void OnSpawnPlayer(SpawnPlayerEvent spawnPlayerEvent)
        {
            var go = Instantiate(playerPrefab, spawnPlayerEvent.SpawnPosition, transform.rotation, gameObject.transform.parent);
            go.name = ConstValues.PLAYER_NAME;
        }
    }
}
