using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IEnemyMovementService
    {
        void StartMovingEnemies(ConstValues.EnemyDirectionSense startingDirectionSense);
        void FinishLevel();
        List<string> GetEnemiesAbleToShoot();
    }
}

