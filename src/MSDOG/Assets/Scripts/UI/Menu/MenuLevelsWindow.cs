using System.Collections.Generic;
using Services;
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

        private readonly List<MenuLevelButton> _buttons = new List<MenuLevelButton>();

        private IObjectResolver _container;
        private ProgressService _progressService;
        private DataService _dataService;

        [Inject]
        public void Construct(IObjectResolver container, ProgressService progressService, DataService dataService)
        {
            _container = container;
            _dataService = dataService;
            _progressService = progressService;
        }

        private void OnEnable()
        {
            _backButton.onClick.AddListener(Hide);
        }

        private void OnDisable()
        {
            _backButton.onClick.RemoveListener(Hide);
        }

        public void Show()
        {
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