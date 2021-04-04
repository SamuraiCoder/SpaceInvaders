using Data;

namespace Services.Interfaces
{
    public interface IGameDirector
    {
        void StartLevel(LevelDefinitionData levelData);
        void FinishLevel();
    }
}
