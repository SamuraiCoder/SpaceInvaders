using System;
using System.Collections.Generic;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class GameEntitiesPositionService : IPositionService
    {
        private Dictionary<string, Vector2> entitiesPositionsByKey = new Dictionary<string, Vector2>();

        public Dictionary<string, Vector2> GetAllEntities() => entitiesPositionsByKey;
        
        public void RegisterEntityPosition(string entityName, Vector2 entityPosition)
        {
            entitiesPositionsByKey[entityName] = entityPosition;
        }

        public void UnRegisterEntityPosition(string entityName)
        {
            DoesEntityExist(entityName);
            
            entitiesPositionsByKey.Remove(entityName);
        }

        public Vector2 GetEntityPosition(string entityName)
        {
            DoesEntityExist(entityName);

            return entitiesPositionsByKey[entityName];
        }

        private void DoesEntityExist(string entityName)
        {
            if (!entitiesPositionsByKey.ContainsKey(entityName))
            {
                throw new Exception($"Position for entity {entityName} not found!");
            }
        }
    }
}


