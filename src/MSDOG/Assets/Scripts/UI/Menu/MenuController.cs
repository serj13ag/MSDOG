using Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Transform _canvasTransform;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private MenuLevelsWindow _menuLevelsWindow;

        private WindowService _windowService;

        [Inject]
        public void Construct(WindowService windowService)
        {
            _windowService = windowService;
        }

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
            _windowService.ShowOptions(_canvasTransform);
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}