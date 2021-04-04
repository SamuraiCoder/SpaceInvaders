namespace Services.Interfaces
{
    public interface IScoreService
    {
        void AddScore(int level, int score);
        void LoadLevelScore(int level);
        void SaveLevelScore(string player, int level);
    }
}
