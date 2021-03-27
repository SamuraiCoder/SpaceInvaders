using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class SpaceInvadersEnemySpawnerService : IEnemySpawnerService
    {
        [Inject] public IPositionService gameEntitiesPositionService;
        
        private int currentSpawnedEntities;
        private int currentRow;
        private int enemiesPerRow;
        private Vector2 initialSpawnPosition;
        
        public SpaceInvadersEnemySpawnerService(IPositionService gameEntitiesPositionService)
        {
            this.gameEntitiesPositionService = gameEntitiesPositionService;
        }
        
        public void SpawnEnemiesByLevel(LevelDefinitionData levelData)
        {
            initialSpawnPosition = gameEntitiesPositionService.GetEntityPosition("StartingPosition");
            
            var amountEnemies = levelData.NumEnemies;
            enemiesPerRow = levelData.EnemiesPerRow;

            for (var i = 0; i < amountEnemies; ++i)
            {
                OnSpawnEnemy(i);
            }
        }
        
        private void OnSpawnEnemy(int position)
        {
            var calculateSpawningPosition = GetSpawnPosition();
            EventBus<SpawnEnemyEvent>.Raise(new SpawnEnemyEvent
            {
                SpawnPosition = calculateSpawningPosition,
                EnemyName = $"{ConstValues.ENEMY_PREFIX_NAME}{position}"
            });

            ++currentSpawnedEntities;
        }

        private Vector2 GetSpawnPosition()
        {
            if (currentSpawnedEntities == 0)
            {
                return initialSpawnPosition;
            }

            var currentColumn = currentSpawnedEntities % enemiesPerRow; 
            
            if (currentColumn == 0)
            {
                ++currentRow;
            }
            
            return new Vector2(initialSpawnPosition.x + currentColumn * ConstValues.SPACING_ENEMY_COLUMN, 
                               initialSpawnPosition.y - currentRow * ConstValues.SPACING_ENEMY_ROW);
        }
    }
}
