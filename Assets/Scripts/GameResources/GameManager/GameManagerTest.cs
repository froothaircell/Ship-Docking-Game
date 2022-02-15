using CoreResources;
using CoreResources.Events;
using UnityEngine;
using UnityEngine.UI;

namespace GameResources.GameManager
{
    public class GameManagerTest : MonoBehaviour
    {
        public Button MainMenu;
        public Button Play;
        public Button Pause;
        public Button Win;
        public Button Loss;
        
        private void Start()
        {
            MainMenu.onClick.AddListener(REvent_GameManagerMainMenu.Dispatch);
            Play.onClick.AddListener(REvent_GameManagerPlay.Dispatch);
            Pause.onClick.AddListener(REvent_GameManagerPause.Dispatch);
            Win.onClick.AddListener(REvent_GameManagerWin.Dispatch);
            Loss.onClick.AddListener(REvent_GameManagerLoss.Dispatch);
        }
    }
}