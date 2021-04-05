using System;
using System.Collections.Generic;
using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Utils;

namespace Services
{
    public class ScoreManagerService : IScoreService
    {
        internal Dictionary<int, int> scorePerLevel;
        internal int currentLevelScore;
        
        public ScoreManagerService()
        {
            scorePerLevel = new Dictionary<int, int>();
        }

        public void StartLevel(int level)
        {
            LoadLevelScore(level);
            currentLevelScore = 0;
        }

        public void AddScore(int level, int score)
        {
            currentLevelScore += score;
            
            EventBus<PlayerScoreAmountEvent>.Raise(new PlayerScoreAmountEvent
            {
                Score = currentLevelScore
            });
        }

        public int GetCurrentScore(int level)
        {
            return currentLevelScore;
        }

        public void LoadLevelScore(int level)
        {
            if (PlayerPrefs.HasKey(ConstValues.SCORE_PREFS_KEY))
            {
                var serializedJsonStr = PlayerPrefs.GetString(ConstValues.SCORE_PREFS_KEY);
                var scoreDict = Json.Deserialize(serializedJsonStr) as Dictionary<string, object>;
                foreach (var scoreElement in scoreDict)
                {
                    var savedLevel = Convert.ToInt32(scoreElement.Key);
                    var levelScoreData = Convert.ToInt32(scoreElement.Value);
                    scorePerLevel[savedLevel] = levelScoreData;
                }
            }
        }

        public void SaveLevelScore(int level)
        {
            if (!scorePerLevel.ContainsKey(level))
            {
                scorePerLevel[level] = currentLevelScore;
                currentLevelScore = 0;
                SaveToDisk();
                return;
            }
            
            var oldSavedScore = scorePerLevel[level];
            
            if (oldSavedScore >= currentLevelScore)
            {
                return;
            }

            scorePerLevel[level] = currentLevelScore;

            currentLevelScore = 0;
            
            SaveToDisk();
        }

        private void SaveToDisk()
        {
            var serializedScore = Json.Serialize(scorePerLevel);
            PlayerPrefs.SetString(ConstValues.SCORE_PREFS_KEY, serializedScore);
            PlayerPrefs.Save();
        }
    }
}
