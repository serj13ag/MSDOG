using Core.Controllers;
using Core.Services;
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
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitGameButton;
        [SerializeField] private MenuLevelsWindow _menuLevelsWindow;

        private WindowController _windowController;
        private SoundController _soundController;
        private IDataService _dataService;

        [Inject]
        public void Construct(WindowController windowController, SoundController soundController, IDataService dataService)
        {
            _dataService = dataService;
            _soundController = soundController;
            _windowController = windowController;
        }

        private void OnEnable()
        {
            _soundController.PlayMusic(_dataService.GetSoundSettingsData().MenuMusic);

            _startGameButton.onClick.AddListener(StartGame);
            _optionsButton.onClick.AddListener(ShowOptions);
            _creditsButton.onClick.AddListener(ShowCredits);
            _quitGameButton.onClick.AddListener(Quit);
        }

        private void OnDisable()
        {
            _startGameButton.onClick.RemoveListener(StartGame);
            _optionsButton.onClick.RemoveListener(ShowOptions);
            _creditsButton.onClick.RemoveListener(ShowCredits);
            _quitGameButton.onClick.RemoveListener(Quit);
        }

        private void StartGame()
        {
            _menuLevelsWindow.Show();
        }

        private void ShowOptions()
        {
            _windowController.ShowOptions();
        }

        private void ShowCredits()
        {
            _windowController.ShowCreditsWindow();
        }

        private void Quit()
        {
            Application.Quit();
        }
    }
}