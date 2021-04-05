using Data;
using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;
using Zenject;

namespace UI
{
    [RequireComponent(typeof(Animator))]
    public class StartScreenController : MonoBehaviour, IEventReceiver<ExitLevelEvent>
    {
        private static readonly int FadeInUi = Animator.StringToHash("FadeIn");
        private static readonly int FadeOutUi = Animator.StringToHash("FadeOut");
        
        [Inject] public IGameDirector gameDirector;
        private Animator uiAnimator;

        private void Start()
        {
            uiAnimator = GetComponent<Animator>();
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnPlayButtonClicked()
        {
            var levelData = new LevelDefinitionData
            {
                LevelNumber = 1,
                EnemiesPerRow = 5,
                NumEnemies = 2,
                EnemyShootPace = 2.5f,
                PlayerLifes = 1,
                ShieldsAmount = 1,
                ShieldHitsPerBlock = 2,
                BonusTimer = 40
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

        public void OnEvent(ExitLevelEvent e)
        {
            uiAnimator.SetTrigger(FadeOutUi);
            gameDirector.FinishLevel(false);
        }
    }
}
