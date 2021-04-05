using Data;

namespace Services.Interfaces
{
    public interface ISpawnerService
    {
        void SpawnEnemiesByLevel(LevelDefinitionData levelData);
        void SpawnPlayer();
        void SpawnShields(LevelDefinitionData levelData);
        void Finishlevel();
    }
}
