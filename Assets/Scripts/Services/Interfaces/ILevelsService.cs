using Data;

namespace Services.Interfaces
{
    public interface ILevelsService
    {
        int GetNextLevelToPlay();
        void FinishLevel(bool levelCompleted);
        LevelDefinitionData GetLevelDefinition(int level);
    }
}
