using Data;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class StartScreenController : MonoBehaviour
    {
        private static readonly int FadeInUi = Animator.StringToHash("FadeIn");
        private static readonly int FadeOutUi = Animator.StringToHash("FadeOut");
        
        [Inject] public IGameDirector gameDirector;
        private Animator uiAnimator;

        private void Start()
        {
            uiAnimator = GetComponent<Animator>();
        }

        public void OnPlayButtonClicked()
        {
            var levelData = new LevelDefinitionData
            {
                LevelNumber = 1,
                EnemiesPerRow = 5,
                NumEnemies = 20,
                EnemyShootPace = 2.5f,
                PlayerLifes = 2,
                ShieldsAmount = 1,
                ShieldHitsPerBlock = 2
            };
        
            gameDirector.StartLevel(levelData);
            
            uiAnimator.SetTrigger(FadeInUi);
        }

        public void OnLeaderboardButtonClicked()
        {
            //Not implemented yet
        }

        public void OnFadeInAnimationEventFinished()
        {
            //In case needed
        }
    }
}
