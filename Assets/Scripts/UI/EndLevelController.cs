using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class EndLevelController : MonoBehaviour, IEventReceiver<ShowEndLevelPanelEvent>
    {
        [SerializeField] private GameObject endLevelPanel;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Text endLevelConditionText;
        [SerializeField] private Text scoreText;

        private void Start()
        {
            EventBus.Register(this);
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(ShowEndLevelPanelEvent e)
        {
            OnShowPanel();
            
            if (e.DidWinLevel)
            {
                endLevelConditionText.color = Color.green;
                endLevelConditionText.text = ConstValues.WIN_TEXT;
                scoreText.text = e.TotalScore.ToString();
            }
            else
            {
                endLevelConditionText.color = Color.red;
                endLevelConditionText.text = ConstValues.LOSE_TEXT;
                scoreText.text = "0";
            }
        }
    
        public void OnContinueButton()
        {
            EventBus<PauseEvent>.Raise(new PauseEvent
            {
                Pause = false
            });
            
            EventBus<ExitLevelEvent>.Raise(new ExitLevelEvent());
            
            OnHideEndLevelPanel();
        }
        
        private void OnShowPanel()
        {
            endLevelPanel.SetActive(true);

            LeanTween.alpha(backgroundImage.rectTransform, 0.8f, 0.5f).setEase(LeanTweenType.linear).setOnComplete(() =>
            {
            });
        }
    
        private void OnHideEndLevelPanel()
        {
            endLevelPanel.SetActive(false);
            LeanTween.alpha(backgroundImage.rectTransform, 0f, 0.5f).setEase(LeanTweenType.linear);
        }
    }
}
