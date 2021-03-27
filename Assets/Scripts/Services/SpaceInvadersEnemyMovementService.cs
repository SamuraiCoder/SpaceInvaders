using System.Collections.Generic;
using Data;
using Events;
using pEventBus;
using Services.Interfaces;
using Zenject;

namespace Services
{
    public class SpaceInvadersEnemyMovementService : IEnemyMovementService, ITickable, IEventReceiver<SpawnEnemyEvent>,
        IEventReceiver<EnemyBorderEvent>
    {
        [Inject] public IPositionService gameEntitiesPositionService;

        private Dictionary<int, EnemyData> enemiesList;
        private bool needsChangeDirection;
        
        public SpaceInvadersEnemyMovementService(IPositionService gameEntitiesPositionService)
        {
            this.gameEntitiesPositionService = gameEntitiesPositionService;

            enemiesList = new Dictionary<int, EnemyData>();
        }
        
        public void StartMovingEnemies()
        {
            
        }

        public void StopMovingEnemies()
        {
            
        }

        public void Tick()
        {
            
        }

        public void OnEvent(SpawnEnemyEvent e)
        {
            enemiesList[e.EnemyPosition] = new EnemyData
            {
                EnemyColor = e.Color,
                EnemyName = e.EnemyName,
                IsEnemyDead = false
            };
        }

        public void OnEvent(EnemyBorderEvent e)
        {
            needsChangeDirection = true;
        }
    }
}
