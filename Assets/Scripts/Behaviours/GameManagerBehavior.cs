using System.Collections;
using Data;
using Services.Interfaces;
using UnityEngine;
using Zenject;

public class GameManagerBehavior : MonoBehaviour
{
    [Inject] public IGameDirector gameDirector;
    
    private void Start()
    {
        StartCoroutine(OnDelayedStart());
    }

    private IEnumerator OnDelayedStart()
    {
        yield return new WaitForSeconds(0.5f);
        
        var levelData = new LevelDefinitionData
        {
            LevelNumber = 1,
            EnemiesPerRow = 5,
            NumEnemies = 20,
            EnemyShootPace = 2.5f,
            PlayerLifes = 2,
            ShieldsAmount = 2,
            ShieldHitsPerBlock = 2
        };
        
        gameDirector.StartLevel(levelData);
    }
}
