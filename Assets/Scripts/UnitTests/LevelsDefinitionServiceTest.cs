using NUnit.Framework;
using Services;
using UnityEngine;

namespace UnitTests
{
    public class LevelsDefinitionServiceTests
    {
        [Test]
        public void LevelDefinitionStart()
        {
            PlayerPrefs.DeleteAll();
            var levelDefService = new LevelsDefinitionService();
            var level = levelDefService.GetNextLevelToPlay();
            Assert.AreEqual(level, 1);
        }
        [Test]
        public void LevelDefinitionGetLevelDefinition()
        {
            PlayerPrefs.DeleteAll();
            var levelDefService = new LevelsDefinitionService();
            var level = levelDefService.GetNextLevelToPlay();
            var getLevelDef = levelDefService.GetLevelDefinition(level);
            Assert.AreEqual(getLevelDef.LevelNumber, 1);
        }
    }
}
