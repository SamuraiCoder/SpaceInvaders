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
        [Inject] public ILevelsService levelsService;
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
            var levelToPlay = levelsService.GetNextLevelToPlay();
            var levelDef = levelsService.GetLevelDefinition(levelToPlay);
            gameDirector.StartLevel(levelDef);
            
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
