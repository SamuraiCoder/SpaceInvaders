using Data;

namespace Services.Interfaces
{
    public interface IShipSpawnerService
    {
        void SpawnEnemiesByLevel(LevelDefinitionData levelData);
        void SpawnPlayer();
    }
}
