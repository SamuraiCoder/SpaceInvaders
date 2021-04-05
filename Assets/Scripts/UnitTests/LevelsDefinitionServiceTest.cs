using NUnit.Framework;
using Services;

namespace UnitTests
{
    public class LevelsDefinitionServiceTests
    {
        [Test]
        public void LevelDefinitionStart()
        {
            var levelDefService = new LevelsDefinitionService();
            var level = levelDefService.GetNextLevelToPlay();
            Assert.AreEqual(level, 1);
        }
        [Test]
        public void LevelDefinitionGetLevelDefinition()
        {
            var levelDefService = new LevelsDefinitionService();
            var level = levelDefService.GetNextLevelToPlay();
            var getLevelDef = levelDefService.GetLevelDefinition(level);
            Assert.AreEqual(getLevelDef.LevelNumber, 1);
        }
    }
}
