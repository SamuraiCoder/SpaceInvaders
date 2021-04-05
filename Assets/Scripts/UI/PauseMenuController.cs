using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PauseMenuController : MonoBehaviour, IEventReceiver<ShowPauseMenuEvent>
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private Image backgroundImage;
        private Canvas canvas;
        
        private void Start()
        {
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(ShowPauseMenuEvent e)
        {
            OnShowPanel();
        }

        public void OnContinueButton()
        {
            EventBus<PauseEvent>.Raise(new PauseEvent
            {
                Pause = false
            });
            
            OnHidePausePanel();
        }

        public void OnMainMenuButton()
        {
            EventBus<PauseEvent>.Raise(new PauseEvent
            {
                Pause = false
            }); 
            
            EventBus<ExitLevelEvent>.Raise(new ExitLevelEvent());
            
            OnHidePausePanel();
        }

        private void OnShowPanel()
        {
            pausePanel.SetActive(true);

            LeanTween.alpha(backgroundImage.rectTransform, 0.8f, 0.5f).setEase(LeanTweenType.linear).setOnComplete(() =>
            {
                EventBus<PauseEvent>.Raise(new PauseEvent
                {
                    Pause = true
                }); 
            });
        }

        private void OnHidePausePanel()
        {
            pausePanel.SetActive(false);
            LeanTween.alpha(backgroundImage.rectTransform, 0f, 0.5f).setEase(LeanTweenType.linear);
        }
    }
}
