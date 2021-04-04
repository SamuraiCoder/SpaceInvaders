using Events;
using pEventBus;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
    public class SetLifesTextBehavior : MonoBehaviour, IEventReceiver<PlayerLifesAmountEvent>
    {
        private Text lifesText;
        
        private void Start()
        {
            EventBus.Register(this);
            lifesText = gameObject.GetComponent<Text>();
        }

        private void OnDestroy()
        {
            EventBus.UnRegister(this);
        }

        public void OnEvent(PlayerLifesAmountEvent e)
        {
            lifesText.text = e.Lifes.ToString();
        }
    }
}
