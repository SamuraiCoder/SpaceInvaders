using Services.Interfaces;
using Zenject;

namespace Services
{
    public class SpaceInvadersEnemyMovementService : IEnemyMovementService
    {
        [Inject] public IPositionService gameEntitiesPositionService;
        
        public SpaceInvadersEnemyMovementService(IPositionService gameEntitiesPositionService)
        {
            this.gameEntitiesPositionService = gameEntitiesPositionService;
        }
        
        public void StartMovingEnemies()
        {
            throw new System.NotImplementedException();
        }

        public void StopMovingEnemies()
        {
            throw new System.NotImplementedException();
        }
    }
}
