using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Services
{
    public class SpaceInvadersDirectorService : IEnemyDirector, ITickable, IEventReceiver<PlayerShipDestroyedEvent>
    {
        [Inject] private IEnemyMovementService enemyMovementService;
        [Inject] private IEnemySpawnerService enemySpanwerService;
        
        private bool hasStarted;
        private float lastCheckAiShoot;
        private float paceCheckAiShoot;
        private Random random;

        public SpaceInvadersDirectorService(IEnemyMovementService enemyMovementService, IEnemySpawnerService enemySpanwerService)
        {
            this.enemyMovementService = enemyMovementService;
            this.enemySpanwerService = enemySpanwerService;
            random = new Random();
        }
        public void StartLevel(LevelDefinitionData levelData)
        {
            enemySpanwerService.SpawnEnemiesByLevel(levelData);
            enemyMovementService.StartMovingEnemies(ConstValues.EnemyDirectionSense.GOING_RIGHT);
            paceCheckAiShoot = levelData.EnemyShootPace;
            hasStarted = true;
        }

        public void FinishLevel()
        {
            hasStarted = false;
        }

        public void Tick()
        {
            if (!hasStarted)
            {
                return;
            }

            AiShoot();
        }

        private void AiShoot()
        {
            lastCheckAiShoot += Time.smoothDeltaTime;

            if (lastCheckAiShoot < paceCheckAiShoot)
            {
                return;
            }

            lastCheckAiShoot = 0.0f;

            OnEnemyShoot();
        }

        private void OnEnemyShoot()
        {
            var enemiesPool = enemyMovementService.GetEnemiesAbleToShoot();
            var rndIdx = random.Next(0, enemiesPool.Count);
            var chosenEnemyName = enemiesPool[rndIdx];
            EventBus<ShootLaserEnemyEvent>.Raise(new ShootLaserEnemyEvent
            {
                EnemyShipName = chosenEnemyName
            });
        }

        public void OnEvent(PlayerShipDestroyedEvent e)
        {
            throw new System.NotImplementedException();
        }
    }
}
