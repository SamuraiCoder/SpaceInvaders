using Data;
using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Services
{
    public class SpaceInvadersDirectorService : IGameDirector, ITickable, IEventReceiver<PlayerShipDestroyedEvent>,
        IEventReceiver<EnemyDestroyedScoreEvent>
    {
        [Inject] private IEnemyMovementService enemyMovementService;
        [Inject] private ISpawnerService spawnerService;
        [Inject] private IScoreService scoreManagerService;
        
        private bool hasStarted;
        private float lastCheckAiShoot;
        private float paceCheckAiShoot;
        private Random random;
        private int currentLifes;
        private int currentLevel;

        public SpaceInvadersDirectorService(IEnemyMovementService enemyMovementService, ISpawnerService spawnerService,
            IScoreService scoreManagerService)
        {
            this.enemyMovementService = enemyMovementService;
            this.spawnerService = spawnerService;
            this.scoreManagerService = scoreManagerService;
            random = new Random();
            EventBus.Register(this);
        }
        public void StartLevel(LevelDefinitionData levelData)
        {
            OnStartEnemiesActions(levelData);

            OnLevelActions(levelData);
            
            OnSpawnPlayerShip();
            
            hasStarted = true;
        }
        
        public void FinishLevel()
        {
            enemyMovementService.FinishLevel();
            spawnerService.Finishlevel();
            DestroyPlayerShip();
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
            if (currentLifes <= 0)
            {
                //GameOver
                FinishLevel();
                
                scoreManagerService.SaveLevelScore(currentLevel);
                return;
            }

            --currentLifes;
            OnSpawnPlayerShip();
        }

        private void OnSpawnPlayerShip()
        {
            spawnerService.SpawnPlayer();
            EventBus<PlayerLifesAmountEvent>.Raise(new PlayerLifesAmountEvent
            {
                Lifes = currentLifes
            });
        }

        public void OnEvent(EnemyDestroyedScoreEvent e)
        {
            scoreManagerService.AddScore(currentLevel, e.ScorePerDeath);
        }
        
        private void DestroyPlayerShip()
        {
            EventBus<DestroyPlayerShip>.Raise(new DestroyPlayerShip());
        }

        private void OnStartEnemiesActions(LevelDefinitionData levelData)
        {
            spawnerService.SpawnEnemiesByLevel(levelData);
            enemyMovementService.StartMovingEnemies(ConstValues.EnemyDirectionSense.GOING_RIGHT);
            paceCheckAiShoot = levelData.EnemyShootPace;
        }
        
        private void OnLevelActions(LevelDefinitionData levelData)
        {
            currentLifes = levelData.PlayerLifes;
            currentLevel = levelData.LevelNumber;
            
            //Reset score
            EventBus<PlayerScoreAmountEvent>.Raise(new PlayerScoreAmountEvent
            {
                Score = 000
            });
            
            spawnerService.SpawnShields(levelData);
        }
    }
}
