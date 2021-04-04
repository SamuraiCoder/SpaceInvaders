using Events;
using pEventBus;
using Services.Interfaces;
using UnityEngine;

namespace Services
{
    public class PauseService : IPauseService, IEventReceiver<PauseEvent>
    {
        public PauseService()
        {
            EventBus.Register(this);
        }

        public void OnEvent(PauseEvent e)
        {
            if (e.Pause)
            {
                Time.timeScale = 0f;
                return;
            }

            Time.timeScale = 1f;
        }
    }
}
