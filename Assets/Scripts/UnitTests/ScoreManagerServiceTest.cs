using NUnit.Framework;
using Services;

namespace UnitTests
{
    public class ScoreManagerServiceTest 
    {
        [Test]
        public void StartLevel()
        {
            var scoreService = new ScoreManagerService();
            scoreService.StartLevel(1);
            Assert.AreEqual(scoreService.currentLevelScore, 0);
        }
        
        [Test]
        public void StartLevelDict()
        {
            var scoreService = new ScoreManagerService();
            scoreService.StartLevel(1);
            Assert.NotNull(scoreService.scorePerLevel);
        }
        
        [Test]
        public void AddScore()
        {
            var scoreService = new ScoreManagerService();
            scoreService.StartLevel(1);
            scoreService.AddScore(1, 10);
            Assert.AreEqual(scoreService.currentLevelScore, 10);
            scoreService.AddScore(1, 10);
            Assert.AreEqual(scoreService.currentLevelScore, 20);
        }
        
        [Test]
        public void GetCurrentScore()
        {
            var scoreService = new ScoreManagerService();
            scoreService.StartLevel(1);
            scoreService.AddScore(1, 10);
            scoreService.AddScore(1, 10);
            var currentScore = scoreService.GetCurrentScore(1);
            Assert.AreEqual(currentScore, 20);
        }
    }
}
