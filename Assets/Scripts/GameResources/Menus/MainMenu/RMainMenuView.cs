using CoreResources.Utils;
using UnityEngine.UI;

namespace GameResources.Menus.MainMenu
{
    public class RMainMenuView : MenuView<RMainMenuView>
    {
        public Button StartGameButton;
        public Button QuitGameButton;

        public override void RemoveAllListeners()
        {
            StartGameButton.onClick.RemoveAllListeners();
            QuitGameButton.onClick.RemoveAllListeners();
        }
    }
}