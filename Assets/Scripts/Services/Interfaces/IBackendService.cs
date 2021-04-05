namespace Services.Interfaces
{
    public interface IBackendService
    {
        void RegisterPlayer(string player);
        void GetLeaderBoards(string country, string token);
        void SubmitScoreLeaderBoards(string player, string score);
    }
}
