using System;
using System.Collections.Generic;
using Data;
using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;
using Random = System.Random;

namespace Services
{
    public class SpaceInvadersSpawnerService : ISpawnerService
    {
        [Inject] public IPositionService gameEntitiesPositionService;
        
        private int currentSpawnedEntities;
        private int currentRow;
        private int enemiesPerRow;
        private Vector2 initialSpawnPosition;
        private List<string> enemySpritesPoolBlack;
        private List<string> enemySpritesPoolBlue;
        private List<string> enemySpritesPoolGreen;
        private List<string> enemySpritesPoolRed;
        private Random random;
        
        public SpaceInvadersSpawnerService(IPositionService gameEntitiesPositionService)
        {
            this.gameEntitiesPositionService = gameEntitiesPositionService;

            CreateEnemySpritesPools();

            random = new Random();
        }

        private void CreateEnemySpritesPools()
        {
            enemySpritesPoolBlack = new List<string>
            {
                ConstValues.ENEMY_BLACK_1, 
                ConstValues.ENEMY_BLACK_2, 
                ConstValues.ENEMY_BLACK_3,
                ConstValues.ENEMY_BLACK_4,
                ConstValues.ENEMY_BLACK_5
            };
            
            enemySpritesPoolBlue = new List<string>
            {
                ConstValues.ENEMY_BLUE_1, 
                ConstValues.ENEMY_BLUE_2, 
                ConstValues.ENEMY_BLUE_3,
                ConstValues.ENEMY_BLUE_4,
                ConstValues.ENEMY_BLUE_5
            };
            
            enemySpritesPoolGreen = new List<string>
            {
                ConstValues.ENEMY_GREEN_1, 
                ConstValues.ENEMY_GREEN_2, 
                ConstValues.ENEMY_GREEN_3,
                ConstValues.ENEMY_GREEN_4,
                ConstValues.ENEMY_GREEN_5
            };
            
            enemySpritesPoolRed = new List<string>
            {
                ConstValues.ENEMY_RED_1, 
                ConstValues.ENEMY_RED_2, 
                ConstValues.ENEMY_RED_3,
                ConstValues.ENEMY_RED_4,
                ConstValues.ENEMY_RED_5
            };
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

        public void SpawnPlayer()
        {
            var playerStartingPosition = gameEntitiesPositionService.GetEntityPosition("StartingPlayerPosition");
            EventBus<SpawnPlayerEvent>.Raise(new SpawnPlayerEvent
            {
                SpawnPosition = playerStartingPosition
            });
        }

        public void SpawnShields(LevelDefinitionData levelData)
        {
            var hitsPerBlock = levelData.ShieldHitsPerBlock;
            if (levelData.ShieldsAmount == 1)
            {
                var shieldSpawnPos = gameEntitiesPositionService.GetEntityPosition("ShieldPosition2");
                OnSpawnShield(shieldSpawnPos, hitsPerBlock);
            }
            else if (levelData.ShieldsAmount == 2)
            {
                var shieldSpawnPos1 = gameEntitiesPositionService.GetEntityPosition("ShieldPosition1");
                OnSpawnShield(shieldSpawnPos1, hitsPerBlock);
                var shieldSpawnPos2 = gameEntitiesPositionService.GetEntityPosition("ShieldPosition3");
                OnSpawnShield(shieldSpawnPos2, hitsPerBlock);
            }
            else if (levelData.ShieldsAmount == 3)
            {
                var shieldSpawnPos1 = gameEntitiesPositionService.GetEntityPosition("ShieldPosition1");
                OnSpawnShield(shieldSpawnPos1, hitsPerBlock);
                var shieldSpawnPos2 = gameEntitiesPositionService.GetEntityPosition("ShieldPosition2");
                OnSpawnShield(shieldSpawnPos2, hitsPerBlock);
                var shieldSpawnPos3 = gameEntitiesPositionService.GetEntityPosition("ShieldPosition3");
                OnSpawnShield(shieldSpawnPos3, hitsPerBlock);
            }
        }

        private void OnSpawnShield(Vector2 spawnPosition, int hitsPerBlock)
        {
            EventBus<SpawnShieldEvent>.Raise(new SpawnShieldEvent
            {
                SpawnPosition = spawnPosition,
                ShieldHitsPerBlock = hitsPerBlock
            });
        }

        private void OnSpawnEnemy(int position)
        {
            var calculateSpawningPosition = GetSpawnPosition();
            var rndColor = GetRandomColor();
            var rndSpriteColor = GetRandomSpriteByColor(rndColor);
            
            EventBus<SpawnEnemyEvent>.Raise(new SpawnEnemyEvent
            {
                SpawnPosition = calculateSpawningPosition,
                EnemyName = $"{ConstValues.ENEMY_PREFIX_NAME}{position}",
                SpriteString = rndSpriteColor,
                Color = rndColor
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
        
        private ConstValues.ColorEnemyPool GetRandomColor()
        {
            var enumValues = Enum.GetValues(typeof(ConstValues.ColorEnemyPool));
            var randomIndex = random.Next(enumValues.Length);
            return (ConstValues.ColorEnemyPool) enumValues.GetValue(randomIndex);
        }
        
        private string GetRandomSpriteByColor(ConstValues.ColorEnemyPool colorEnemyPool)
        {
            switch (colorEnemyPool)
            {
                case ConstValues.ColorEnemyPool.BLACK:
                {
                    var randomIndex = random.Next(enemySpritesPoolBlack.Count);
                    return enemySpritesPoolBlack[randomIndex];
                }
                case ConstValues.ColorEnemyPool.BLUE:
                {
                    var randomIndex = random.Next(enemySpritesPoolBlue.Count);
                    return enemySpritesPoolBlue[randomIndex];
                }
                case ConstValues.ColorEnemyPool.GREEN:
                {
                    var randomIndex = random.Next(enemySpritesPoolGreen.Count);
                    return enemySpritesPoolGreen[randomIndex];
                }
                case ConstValues.ColorEnemyPool.RED:
                {
                    var randomIndex = random.Next(enemySpritesPoolRed.Count);
                    return enemySpritesPoolRed[randomIndex];
                }
            }

            return string.Empty;
        }
    }
}
