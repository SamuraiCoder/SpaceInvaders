using Events;
using pEventBus;
using UnityEngine;

namespace UI
{
    public class MenuButtonController : MonoBehaviour
    {
        public void OnPauseMenu()
        {
            EventBus<ShowPauseMenuEvent>.Raise(new ShowPauseMenuEvent());
        }
    }
}
