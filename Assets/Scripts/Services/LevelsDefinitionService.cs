using System.Collections.Generic;
using Data;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class LevelsDefinitionService : ILevelsService
    {
        private int currentLevel;
        private Dictionary<int, LevelDefinitionData> levelsDefinition;

        public LevelsDefinitionService()
        {
            levelsDefinition = new Dictionary<int, LevelDefinitionData>();
            OnLoginReceivedData();
        }
        
        public int GetNextLevelToPlay()
        {
            var lastPlayedLevel = LoadFromDisk();

            if (lastPlayedLevel >= ConstValues.MAX_AMOUNT_LEVELS)
            {
                currentLevel = 1;
                return currentLevel;
            }
            
            currentLevel = lastPlayedLevel + 1;
            return currentLevel;
        }

        public void FinishLevel(bool levelCompleted)
        {
            if (!levelCompleted)
            {
                return;
            }
            
            SaveToDisk();
        }

        public LevelDefinitionData GetLevelDefinition(int level)
        {
            return levelsDefinition[level];
        }

        private void OnLoginReceivedData()
        {
            //NOTE: This method emulates when receiving data from login/backend
            var levelData1 = new LevelDefinitionData
            {
                LevelNumber = 1,
                EnemiesPerRow = 5,
                NumEnemies = 5,
                EnemyShootPace = 3f,
                PlayerLifes = 3,
                ShieldsAmount = 1,
                ShieldHitsPerBlock = 2,
                BonusTimer = 20
            };
            
            var levelData2 = new LevelDefinitionData
            {
                LevelNumber = 2,
                EnemiesPerRow = 5,
                NumEnemies = 10,
                EnemyShootPace = 3f,
                PlayerLifes = 3,
                ShieldsAmount = 2,
                ShieldHitsPerBlock = 2,
                BonusTimer = 40
            };
            
            var levelData3 = new LevelDefinitionData
            {
                LevelNumber = 3,
                EnemiesPerRow = 5,
                NumEnemies = 15,
                EnemyShootPace = 2f,
                PlayerLifes = 3,
                ShieldsAmount = 1,
                ShieldHitsPerBlock = 4,
                BonusTimer = 60
            };
            
            var levelData4 = new LevelDefinitionData
            {
                LevelNumber = 4,
                EnemiesPerRow = 5,
                NumEnemies = 20,
                EnemyShootPace = 2f,
                PlayerLifes = 2,
                ShieldsAmount = 2,
                ShieldHitsPerBlock = 5,
                BonusTimer = 90
            };
            
            var levelData5 = new LevelDefinitionData
            {
                LevelNumber = 5,
                EnemiesPerRow = 5,
                NumEnemies = 25,
                EnemyShootPace = 1f,
                PlayerLifes = 3,
                ShieldsAmount = 3,
                ShieldHitsPerBlock = 1,
                BonusTimer = 160
            };

            levelsDefinition[1] = levelData1;
            levelsDefinition[2] = levelData2;
            levelsDefinition[3] = levelData3;
            levelsDefinition[4] = levelData4;
            levelsDefinition[5] = levelData5;
        }
        
        private void SaveToDisk()
        {
            PlayerPrefs.SetInt(ConstValues.LEVEL_PLAYED_PREFS_KEY, currentLevel);
            PlayerPrefs.Save();
        }
        
        private int LoadFromDisk()
        {
            if (PlayerPrefs.HasKey(ConstValues.LEVEL_PLAYED_PREFS_KEY))
            {
                return PlayerPrefs.GetInt(ConstValues.LEVEL_PLAYED_PREFS_KEY);
            }

            return 0;
        }
    }
}
