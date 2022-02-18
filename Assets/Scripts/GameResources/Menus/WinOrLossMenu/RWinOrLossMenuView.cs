using CoreResources.Utils;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace GameResources.Menus.WinOrLossMenu
{
    public class RWinOrLossMenuView : MenuView<RWinOrLossMenuView>
    {
        public Button restartLevelButton;
        public Button mainMenuButton;
        public Button nextLevelButton;
        public TMP_Text winOrLossText;

        public override void RemoveAllListeners()
        {
            restartLevelButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.RemoveAllListeners();
            nextLevelButton.onClick.RemoveAllListeners();
        }
    }
}