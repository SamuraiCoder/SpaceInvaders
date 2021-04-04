using System.Collections.Generic;
using Data;
using Events;
using pEventBus;
using Services.Interfaces;

namespace Services
{
    public class ScoreManagerService : IScoreService
    {
        private Dictionary<int, LevelScoreData> scorePerLevel;
        private LevelScoreData currentLevelScore;
        
        public ScoreManagerService()
        {
            scorePerLevel = new Dictionary<int, LevelScoreData>();
        }
        
        public void AddScore(int level, int score)
        {
            if (currentLevelScore == null)
            {
                currentLevelScore = new LevelScoreData
                {
                    Score = score
                };

                return;
            }

            currentLevelScore.Score += score;
            
            EventBus<PlayerScoreAmountEvent>.Raise(new PlayerScoreAmountEvent
            {
                Score = currentLevelScore.Score
            });
        }

        public void LoadLevelScore(int level)
        {
            throw new System.NotImplementedException();
        }

        public void SaveLevelScore(int level)
        {
            if (!scorePerLevel.ContainsKey(level))
            {
                scorePerLevel[level] = currentLevelScore;
                return;
            }
            
            var oldSavedData = scorePerLevel[level];

            var oldScore = oldSavedData.Score;

            if (oldScore >= currentLevelScore.Score)
            {
                currentLevelScore = null;
                return;
            }

            scorePerLevel[level] = currentLevelScore;
            
            //Save to disk?

        }
    }
}
