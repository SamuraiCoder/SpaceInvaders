using System.Collections;
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
            EnemiesPerRow = 5,
            NumEnemies = 20,
            EnemyShootPace = 2.5f
        };
        
        gameDirector.StartLevel(levelData);
    }
}
