using System;
using System.Collections.Generic;
using Data;
using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace Services
{
    public class SpaceInvadersEnemyMovementService : IEnemyMovementService, ITickable, IEventReceiver<SpawnEnemyEvent>,
        IEventReceiver<EnemyBorderEvent>
    {
        [Inject] public IPositionService gameEntitiesPositionService;

        private Dictionary<int, EnemyData> enemiesList;
        private bool touchedLeft;
        private bool touchedRight;
        private bool needsToggleDirection;
        private bool needsToGoDown;
        private bool hasStarted;
        private float lastCheck;
        private ConstValues.EnemyDirectionSense currentEnemyDirection;
        public bool TouchedEvent => touchedLeft || touchedRight;
        
        public SpaceInvadersEnemyMovementService(IPositionService gameEntitiesPositionService)
        {
            this.gameEntitiesPositionService = gameEntitiesPositionService;

            enemiesList = new Dictionary<int, EnemyData>();
            
            EventBus.Register(this);
        }
        
        public void StartMovingEnemies(ConstValues.EnemyDirectionSense startingDirectionSense)
        {
            hasStarted = true;
            currentEnemyDirection = startingDirectionSense;
        }

        public void StopMovingEnemies()
        {
            hasStarted = false;
        }

        public void Tick()
        {
            if (!hasStarted)
            {
                return;
            }
            
            lastCheck += Time.deltaTime;

            if (lastCheck < 1.0f)
            {
                StopEnemies();
                return;
            }

            lastCheck = 0.0f;

            EnemyDirection();
        }

        private void EnemyDirection()
        {
            if (needsToggleDirection)
            {
                Debug.Log("Needs toggle direction");
                OnToggleDirection();
                needsToggleDirection = false;
                needsToGoDown = false;
            }
            else if (needsToGoDown)
            {
                Debug.Log("Needs to go down");
                currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_DOWN;
                needsToggleDirection = true;
            }
            else
            {
                touchedLeft = false;
                touchedRight = false;
                Debug.Log($"Normal movement. Walls: {touchedLeft} {touchedRight}");
            }
            
            OnMoveEnemies();
        }

        private void OnMoveEnemies()
        {
            switch (currentEnemyDirection)
            {
                case ConstValues.EnemyDirectionSense.GOING_RIGHT:
                    MoveEnemies(ConstValues.EnemyDirectionSense.GOING_RIGHT);
                    break;
                case ConstValues.EnemyDirectionSense.GOING_LEFT:
                    MoveEnemies(ConstValues.EnemyDirectionSense.GOING_LEFT);
                    break;
                case ConstValues.EnemyDirectionSense.GOING_DOWN:
                    MoveEnemies(ConstValues.EnemyDirectionSense.GOING_DOWN);
                    break;
            } 
        }

        private void MoveEnemies(ConstValues.EnemyDirectionSense directionSense)
        {
            foreach (var enemy in enemiesList)
            {
                var enemyName = enemy.Value.EnemyName;
                var newPosition = Vector2.zero;

                switch (directionSense)
                {
                    case ConstValues.EnemyDirectionSense.GOING_RIGHT:
                    {
                        newPosition = Vector2.right;
                        break;
                    }
                    case ConstValues.EnemyDirectionSense.GOING_LEFT:
                    {
                        newPosition = Vector2.left;
                        break;
                    }
                    case ConstValues.EnemyDirectionSense.GOING_DOWN:
                    {
                        newPosition = Vector2.down;
                        break;
                    }
                }
                
                EventBus<EnemyShipMoveEvent>.Raise(new EnemyShipMoveEvent
                {
                    EnemyShipName = enemyName,
                    Direction = newPosition,
                    Speed = ConstValues.ENEMY_SPEED
                });
            }
        }

        private void OnToggleDirection()
        {
            switch (currentEnemyDirection)
            {
                case ConstValues.EnemyDirectionSense.GOING_LEFT:
                {
                    currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_RIGHT;
                    break;
                }
                case ConstValues.EnemyDirectionSense.GOING_RIGHT:
                {
                    currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_LEFT;
                    break;
                }
                case ConstValues.EnemyDirectionSense.GOING_DOWN:
                {
                    if (touchedLeft)
                    {
                        currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_RIGHT;
                    }
                    else if (touchedRight)
                    {
                        currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_LEFT;
                    }
                    break;
                }
            }
        }

        private void StopEnemies()
        {
            foreach (var enemy in enemiesList)
            {
                var enemyName = enemy.Value.EnemyName;
                var newPosition = Vector2.zero;
                //Send new position
                EventBus<EnemyShipMoveEvent>.Raise(new EnemyShipMoveEvent
                {
                    EnemyShipName = enemyName,
                    Direction = newPosition,
                    Speed = 0
                });
            } 
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
            touchedLeft = e.EnemyTouchLeft;
            touchedRight = e.EnemyTouchRight;

            if (touchedLeft || touchedRight)
            {
                needsToGoDown = true;
            }

        }
    }
}
