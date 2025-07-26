using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private MenuLevelsWindow _menuLevelsWindow;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(StartGame);
            _optionsButton.onClick.AddListener(ShowOptions);
            _quitGameButton.onClick.AddListener(Quit);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _optionsButton.onClick.RemoveListener(ShowOptions);
            _quitGameButton.onClick.RemoveListener(Quit);
        }

        private void StartGame()
        {
            _menuLevelsWindow.Show();
        }

        private void ShowOptions()
        {
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}