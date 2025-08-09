using System.Collections.Generic;
using Core.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace UI.Menu
{
    public class MenuLevelsWindow : MonoBehaviour
    {
        [SerializeField] private MenuLevelButton _levelButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _backButton;
        [SerializeField] private Button _unlockLevelsButton;
        [SerializeField] private Toggle _easyModeToggle;

        private readonly List<MenuLevelButton> _buttons = new List<MenuLevelButton>();

        private IObjectResolver _container;
        private IProgressService _progressService;
        private IDataService _dataService;

        [Inject]
        public void Construct(IObjectResolver container, IProgressService progressService, IDataService dataService)
        {
            _container = container;
            _dataService = dataService;
            _progressService = progressService;
        }

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Hide);
            _unlockLevelsButton.onClick.AddListener(UnlockLevels);
            _easyModeToggle.onValueChanged.AddListener(EasyModeChanged);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Hide);
            _unlockLevelsButton.onClick.RemoveListener(UnlockLevels);
            _easyModeToggle.onValueChanged.AddListener(EasyModeChanged);
        }

        public void Show()
        {
            _easyModeToggle.isOn = _progressService.EasyModeEnabled;

            var lastPassedLevel = _progressService.LastPassedLevel;

            for (var i = 0; i < _dataService.GetNumberOfLevels(); i++)
            {
                var button = _container.Instantiate(_levelButtonPrefab, _buttonsContainer);
                var levelData = _dataService.GetLevelData(i);
                var isAvailable = i <= lastPassedLevel + 1;
                button.Init(levelData, isAvailable);

                _buttons.Add(button);
            }

            gameObject.SetActive(true);
        }

        private void UnlockLevels()
        {
            _progressService.UnlockAllLevels();

            var lastPassedLevel = _progressService.LastPassedLevel;
            for (var i = 0; i < _buttons.Count; i++)
            {
                var button = _buttons[i];
                var isAvailable = i <= lastPassedLevel + 1;
                button.UpdateIsAvailable(isAvailable);
            }
        }

        private void EasyModeChanged(bool easyModeEnabled)
        {
            _progressService.SetEasyMode(easyModeEnabled);
        }

        private void Hide()
        {
            gameObject.SetActive(false);

            foreach (var button in _buttons)
            {
                Destroy(button.gameObject);
            }

            _buttons.Clear();
        }
    }
}