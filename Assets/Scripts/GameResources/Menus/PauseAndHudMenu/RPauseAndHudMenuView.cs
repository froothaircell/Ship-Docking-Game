using CoreResources.Utils;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

namespace GameResources.Menus.PauseAndHudMenu
{
    public class RPauseAndHudMenuView : MenuView<RPauseAndHudMenuView>
    {
        public Button settingsButton;
        public Button pause_MainMenuButton;
        public TMP_Text levelText;
        public TMP_Text scoreText;
        public GameObject pauseMenu;

        public override void RemoveAllListeners()
        {
            settingsButton.onClick.RemoveAllListeners();
            pause_MainMenuButton.onClick.RemoveAllListeners();
        }
    }
}