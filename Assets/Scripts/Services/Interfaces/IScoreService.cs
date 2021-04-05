namespace Services.Interfaces
{
    public interface IScoreService
    {
        void StartLevel(int level);
        void AddScore(int level, int score);
        int GetCurrentScore(int level);
        void LoadLevelScore(int level);
        void SaveLevelScore(int level);
    }
}
