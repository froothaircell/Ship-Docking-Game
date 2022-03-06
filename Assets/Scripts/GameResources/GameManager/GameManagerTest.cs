using GameResources.Events;
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
            MainMenu.onClick.AddListener(REvent_GameManagerPauseToMainMenu.Dispatch);
            Play.onClick.AddListener(REvent_GameManagerMainMenuToPlay.Dispatch);
            Pause.onClick.AddListener(REvent_GameManagerPlayToPause.Dispatch);
            Win.onClick.AddListener(REvent_GameManagerPlayToWin.Dispatch);
            Loss.onClick.AddListener(REvent_GameManagerPlayToLoss.Dispatch);
        }
    }
}