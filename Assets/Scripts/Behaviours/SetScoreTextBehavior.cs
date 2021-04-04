using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
    public class SetScoreTextBehavior : MonoBehaviour, IEventReceiver<PlayerScoreAmountEvent>
    {
        private Text scoreText;
        
        private void Start()
        {
            EventBus.Register(this);
            scoreText = GetComponent<Text>();
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(PlayerScoreAmountEvent e)
        {
            scoreText.text = e.Score.ToString();
        }
    }
}
