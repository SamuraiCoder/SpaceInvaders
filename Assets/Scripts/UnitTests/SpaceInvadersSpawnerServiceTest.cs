using System.Numerics;
using Data;
using Moq;
using NUnit.Framework;
using Services;
using Services.Interfaces;

public class SpaceInvadersSpawnerServiceTest
{
    [Test]
    public void SpawnerServiceInit()
    {
        var positionServiceMock = new Mock<IPositionService>();
        var spaceInvadersSpawnerService = new SpaceInvadersSpawnerService(positionServiceMock.Object);

        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolBlack.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolBlue.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolGreen.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolRed.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedEntities, 0);
    }
    
    [Test]
    public void SpawnerServiceSpawnEnemiesByLevel()
    {
        var positionServiceMock = new Mock<IPositionService>();
        var spaceInvadersSpawnerService = new SpaceInvadersSpawnerService(positionServiceMock.Object);

        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolBlack.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolBlue.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolGreen.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.enemySpritesPoolRed.Count, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedEntities, 0);
        
        var levelDummy = new LevelDefinitionData
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
        
        spaceInvadersSpawnerService.SpawnEnemiesByLevel(levelDummy);
        
        Assert.NotNull(spaceInvadersSpawnerService.initialSpawnPosition);
        Assert.AreEqual(spaceInvadersSpawnerService.enemiesPerRow, 5);
        Assert.AreEqual(spaceInvadersSpawnerService.currentRow, 0);
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedEntities, 5);
    }
    
    [Test]
    public void SpawnerServiceSpawnShields()
    {
        var positionServiceMock = new Mock<IPositionService>();
        var spaceInvadersSpawnerService = new SpaceInvadersSpawnerService(positionServiceMock.Object);
        
        var levelDummy = new LevelDefinitionData
        {
            LevelNumber = 1,
            EnemiesPerRow = 5,
            NumEnemies = 5,
            EnemyShootPace = 3f,
            PlayerLifes = 3,
            ShieldsAmount = 2,
            ShieldHitsPerBlock = 2,
            BonusTimer = 20
        };
        
        spaceInvadersSpawnerService.SpawnShields(levelDummy);
        
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedShields, 2);
    }
    
    [Test]
    public void SpawnerServiceFinishLevel()
    {
        var positionServiceMock = new Mock<IPositionService>();
        var spaceInvadersSpawnerService = new SpaceInvadersSpawnerService(positionServiceMock.Object);

        spaceInvadersSpawnerService.Finishlevel();
        
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedEntities, 0);
        Assert.AreEqual(spaceInvadersSpawnerService.currentSpawnedShields, 0);
    }
}
