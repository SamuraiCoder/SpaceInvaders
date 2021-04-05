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
        IEventReceiver<EnemyDestroyedScoreEvent>, IEventReceiver<EndLevelConditionEvent>
    {
        [Inject] private IEnemyMovementService enemyMovementService;
        [Inject] private ISpawnerService spawnerService;
        [Inject] private IScoreService scoreManagerService;
        [Inject] private ILevelsService levelsService;
        
        private bool hasStarted;
        private float lastCheckAiShoot;
        private float paceCheckAiShoot;
        private Random random;
        private int currentLifes;
        private int currentLevel;
        private float currentBonusTimer;
        private float totalBonusTimer;
        private bool outOfTimeBonus;

        public SpaceInvadersDirectorService(IEnemyMovementService enemyMovementService, ISpawnerService spawnerService,
            IScoreService scoreManagerService, ILevelsService levelsService)
        {
            this.enemyMovementService = enemyMovementService;
            this.spawnerService = spawnerService;
            this.scoreManagerService = scoreManagerService;
            this.levelsService = levelsService;
            
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
        
        public void FinishLevel(bool isCompleted)
        {
            enemyMovementService.FinishLevel();
            spawnerService.Finishlevel();
            levelsService.FinishLevel(isCompleted);
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
            OnCheckBonusTimer();
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
            if (enemiesPool.Count < rndIdx)
            {
                return;
            }
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
                OnEndLevelActions(false);
                FinishLevel(false);
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
            currentBonusTimer = 0.0f;
            outOfTimeBonus = false;
            currentLifes = levelData.PlayerLifes;
            currentLevel = levelData.LevelNumber;
            totalBonusTimer = levelData.BonusTimer;
            
            //Reset score
            EventBus<PlayerScoreAmountEvent>.Raise(new PlayerScoreAmountEvent
            {
                Score = 000
            });
            
            scoreManagerService.StartLevel(currentLevel);
            
            spawnerService.SpawnShields(levelData);
        }
        
        private void OnCheckBonusTimer()
        {
            if (outOfTimeBonus)
            {
                return;
            }
            
            currentBonusTimer += Time.smoothDeltaTime;

            if (currentBonusTimer < totalBonusTimer)
            {
                return;
            }

            outOfTimeBonus = true;
        }

        public void OnEvent(EndLevelConditionEvent e)
        {
            OnEndLevelActions(e.WinLevel);
            FinishLevel(e.WinLevel);
        }

        private void OnEndLevelActions(bool wonLevel)
        {
            var totalScore = scoreManagerService.GetCurrentScore(currentLevel);
            if (!outOfTimeBonus)
            {
                var extraBonus = 1 + currentBonusTimer / totalBonusTimer;
                var scoreWithBonus = (int)(totalScore * extraBonus);
                scoreManagerService.AddScore(currentLevel, scoreWithBonus);
            }

            var endScore = scoreManagerService.GetCurrentScore(currentLevel);
            
            if (wonLevel)
            {
                totalScore = endScore;
                scoreManagerService.SaveLevelScore(currentLevel);
            }
            else
            {
                totalScore = 0;
            }
            
            EventBus<ShowEndLevelPanelEvent>.Raise(new ShowEndLevelPanelEvent
            {
                DidWinLevel = wonLevel,
                TotalScore = totalScore
            }); 
        }
    }
}
