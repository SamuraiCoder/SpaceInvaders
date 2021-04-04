using System.Collections.Generic;
using Data;
using Services.Interfaces;

namespace Services
{
    public class ScoreManagerService : IScoreService
    {
        private Dictionary<string, Dictionary<int, LevelScoreData>> scorePerLevel;
        private LevelScoreData currentLevelScore;
        
        public ScoreManagerService()
        {
            scorePerLevel = new Dictionary<string, Dictionary<int, LevelScoreData>>();
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
        }

        public void LoadLevelScore(int level)
        {
            throw new System.NotImplementedException();
        }

        public void SaveLevelScore(string player, int level)
        {
            //Temp data from level
            var newScoreData = new Dictionary<int, LevelScoreData> {[level] = currentLevelScore};

            if (!scorePerLevel.ContainsKey(player))
            {
                scorePerLevel[player] = newScoreData;
                return;
            }
            
            var oldSavedData = scorePerLevel[player];

            if (!oldSavedData.ContainsKey(level))
            {
                scorePerLevel[player] = newScoreData;
                return;
            }

            var oldScore = oldSavedData[level].Score;

            if (oldScore >= currentLevelScore.Score)
            {
                currentLevelScore = null;
                return;
            }

            oldSavedData[level] = currentLevelScore;
            
            //Save to disk?

        }
    }
}
