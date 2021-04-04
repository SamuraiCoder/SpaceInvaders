using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Behaviours
{
    public class SpawnerBehavior : MonoBehaviour, IEventReceiver<SpawnEnemyEvent>, IEventReceiver<SpawnPlayerEvent>,
        IEventReceiver<SpawnShieldEvent>
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject[] shieldPrefabs;

        private Random random;

        private void Start()
        {
            EventBus.Register(this);
            random = new Random();
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

        public void OnEvent(SpawnShieldEvent e)
        {
            var rndIdx = random.Next(0, shieldPrefabs.Length);
            var shieldRef = shieldPrefabs[rndIdx];
            var go = Instantiate(shieldRef, e.SpawnPosition, transform.rotation, gameObject.transform.parent);
            OnApplyNumHits(go, e.ShieldHitsPerBlock);
        }

        private void OnApplyNumHits(GameObject go, int shieldHitsPerBlock)
        {
            var children = go.GetComponentsInChildren<ShieldPartBehavior>();
            foreach (var shield in children)
            {
                shield.NumHits = shieldHitsPerBlock;
            }
        }
    }
}
