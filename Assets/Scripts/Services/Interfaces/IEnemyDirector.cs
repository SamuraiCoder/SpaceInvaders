namespace Services.Interfaces
{
    public interface IEnemyDirector
    {
        void StartLevel(LevelDefinitionData levelData);
        void FinishLevel();
    }
}
