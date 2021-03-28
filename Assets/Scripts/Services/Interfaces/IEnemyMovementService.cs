namespace Services.Interfaces
{
    public interface IEnemyMovementService
    {
        void StartMovingEnemies(ConstValues.EnemyDirectionSense startingDirectionSense);
        void StopMovingEnemies();
    }
}

