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
        IEventReceiver<EnemyBorderEvent>, IEventReceiver<EnemySendSurroundingsEvent>, IEventReceiver<EnemyShipDestroyedEvent>
    {
        private Dictionary<string, EnemyData> enemiesList;
        private bool touchedLeft;
        private bool touchedRight;
        private bool needsToggleDirection;
        private bool needsToGoDown;
        private bool hasStarted;
        private float lastCheckAIMove;
        private float paceCheckAIMove;
        private ConstValues.EnemyDirectionSense currentEnemyDirection;
        private float lastCheckAIDetector;
        private float paceCheckAIDetector;
        
        public bool TouchedEvent => touchedLeft || touchedRight;
        
        public SpaceInvadersEnemyMovementService()
        {
            enemiesList = new Dictionary<string, EnemyData>();
            EventBus.Register(this);
            paceCheckAIMove = ConstValues.AI_ENEMY_PACE_CHECK;
            paceCheckAIDetector = ConstValues.AI_ENEMY_NOTIFY_SURROUNDINGS_CHECK;
        }
        
        public void StartMovingEnemies(ConstValues.EnemyDirectionSense startingDirectionSense)
        {
            hasStarted = true;
            currentEnemyDirection = startingDirectionSense;
        }

        public void FinishLevel()
        {
            hasStarted = false;
            OnDeleteEnemiesShip();
            enemiesList.Clear();
        }
        
        public void Tick()
        {
            if (!hasStarted)
            {
                return;
            }

            AICheckWinCondition();
            AIMoveEnemies();
            AiEnemyShipDetector();
        }

        private void AICheckWinCondition()
        {
            if (enemiesList.Count != 0)
            {
                return;
            }
            
            EventBus<EndLevelConditionEvent>.Raise(new EndLevelConditionEvent
            {
                WinLevel = true
            });
            
            hasStarted = false;
        }

        private void AIMoveEnemies()
        {
            lastCheckAIMove += Time.smoothDeltaTime;

            if (lastCheckAIMove < paceCheckAIMove)
            {
                StopEnemies();
                return;
            }

            lastCheckAIMove = 0.0f;

            EnemyMove(); 
        }

        private void EnemyMove()
        {
            if (needsToggleDirection)
            {
                OnToggleDirection();
                needsToggleDirection = false;
                needsToGoDown = false;
            }
            else if (needsToGoDown)
            {
                currentEnemyDirection = ConstValues.EnemyDirectionSense.GOING_DOWN;
                needsToggleDirection = true;
            }
            else
            {
                touchedLeft = false;
                touchedRight = false;
            }
            
            OnMoveEnemiesDirection();
        }

        private void OnMoveEnemiesDirection()
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
        
        private void AiEnemyShipDetector()
        {
            lastCheckAIDetector += Time.smoothDeltaTime;

            if (lastCheckAIDetector < paceCheckAIDetector)
            {
                return;
            }

            lastCheckAIDetector = 0.0f;
            
            //Send broadcast msg to all the ships to inform their surroundings
            foreach (var enemy in enemiesList)
            {
                var enemyName = enemy.Value.EnemyName;
                EventBus<EnemyNotifySurroundingsEvent>.Raise(new EnemyNotifySurroundingsEvent
                {
                    EnemyShipName = enemyName
                });
            }
        }

        public void OnEvent(SpawnEnemyEvent e)
        {
            enemiesList[e.EnemyName] = new EnemyData
            {
                EnemyColor = e.Color,
                EnemyName = e.EnemyName
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

        public void OnEvent(EnemySendSurroundingsEvent e)
        {
            var enemyData = enemiesList[e.EnemyShipName];

            var newEnemyData = new EnemyData
            {
                EnemyName = enemyData.EnemyName,
                EnemyColor = enemyData.EnemyColor,
                FriendEnemyDown = e.EnemyDataInfo.FriendEnemyDown,
                FriendEnemyLeft = e.EnemyDataInfo.FriendEnemyLeft,
                FriendEnemyRight = e.EnemyDataInfo.FriendEnemyRight,
                FriendEnemyUp = e.EnemyDataInfo.FriendEnemyUp,
            };

            enemiesList[e.EnemyShipName] = newEnemyData;
        }
        
        public List<string> GetEnemiesAbleToShoot()
        {
            var enemiesAbleToShoot = new List<string>();

            foreach (var enemy in enemiesList)
            {
                if (enemy.Value.FriendEnemyDown != null)
                {
                    continue;
                }
                
                enemiesAbleToShoot.Add(enemy.Key);
            }

            return enemiesAbleToShoot;
        }

        public void OnEvent(EnemyShipDestroyedEvent e)
        {
            if (!enemiesList.ContainsKey(e.EnemyShipName))
            {
                return;
            }

            var enemyData = enemiesList[e.EnemyShipName];
            var scoreShipDestroyed = GetScoreEnemyShip(enemyData);
            //Add the score 
            EventBus<EnemyDestroyedScoreEvent>.Raise(new EnemyDestroyedScoreEvent
            {
                ScorePerDeath = scoreShipDestroyed
            });
            var enemiesListToDestroy = new List<string>();
            OnCascadeEffect(e.EnemyShipName, enemyData.EnemyColor, ref enemiesListToDestroy);
            //NOTE: if list is empty means it was not a multiple shot. Include single death
            if (enemiesListToDestroy.Count == 0)
            {
                enemiesListToDestroy.Add(enemyData.EnemyName);
            }
            OnDestroyEnemiesEffect(enemiesListToDestroy);
        }
        
        private void OnDestroyEnemiesEffect(List<string> enemiesListToDestroy)
        {
            foreach (var enemyName in enemiesListToDestroy)
            {
                if (!enemiesList.ContainsKey(enemyName))
                {
                    continue;
                }

                var enemyData = enemiesList[enemyName];
                var scoreShipDestroyed = GetScoreEnemyShip(enemyData);
                //Add the score 
                EventBus<EnemyDestroyedScoreEvent>.Raise(new EnemyDestroyedScoreEvent
                {
                    ScorePerDeath = scoreShipDestroyed
                });
                
                //Destroy graphically the ship
                EventBus<EnemyCascadeEffectEvent>.Raise(new EnemyCascadeEffectEvent
                {
                    EnemyShipName = enemyName
                });
                    
                enemiesList.Remove(enemyName);
            }
        }

        private void OnCascadeEffect(string enemyShipName, ConstValues.ColorEnemyPool enemyColor, ref List<string> enemiesListToDestroy)
        {
            var enemyData = enemiesList[enemyShipName];
            var neighbours = GetNeighbours(enemyData);
            foreach (var neighbour in neighbours)
            {
                if (!enemiesList.ContainsKey(neighbour))
                {
                    continue;
                }
                var neighbourData = enemiesList[neighbour];
                if (neighbourData.EnemyColor == enemyColor)
                {
                    if (enemiesListToDestroy.Contains(neighbour))
                    {
                        continue;
                    }
                    enemiesListToDestroy.Add(neighbour);
                    OnCascadeEffect(neighbour, neighbourData.EnemyColor, ref enemiesListToDestroy);
                }
            }
        }

        private List<string> GetNeighbours(EnemyData enemyData)
        {
            var neighbours = new List<string>();

            if (enemyData.FriendEnemyDown != null)
            {
                neighbours.Add(enemyData.FriendEnemyDown);
            }
            
            if (enemyData.FriendEnemyUp != null)
            {
                neighbours.Add(enemyData.FriendEnemyUp);
            }
            
            if (enemyData.FriendEnemyLeft != null)
            {
                neighbours.Add(enemyData.FriendEnemyLeft);
            }
            
            if (enemyData.FriendEnemyRight != null)
            {
                neighbours.Add(enemyData.FriendEnemyRight);
            }

            return neighbours;
        }
        
        private int GetScoreEnemyShip(EnemyData enemyData)
        {
            var score = 0;
            
            switch (enemyData.EnemyColor)
            {
                case ConstValues.ColorEnemyPool.BLACK:
                {
                    score = ConstValues.SCORE_BLACK;
                    break;
                }
                case ConstValues.ColorEnemyPool.BLUE:
                {
                    score = ConstValues.SCORE_BLUE;
                    break;
                }
                case ConstValues.ColorEnemyPool.GREEN:
                {
                    score = ConstValues.SCORE_GREEN;
                    break;
                }
                case ConstValues.ColorEnemyPool.RED:
                {
                    score = ConstValues.SCORE_RED;
                    break;
                }
            }

            return score;
        }

        private void OnDeleteEnemiesShip()
        {
            foreach (var enemyShip in enemiesList)
            {
                //Destroy graphically the ship
                EventBus<EnemyCascadeEffectEvent>.Raise(new EnemyCascadeEffectEvent
                {
                    EnemyShipName = enemyShip.Key
                });
            }
        }
    }
}
