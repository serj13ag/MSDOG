using System.Collections.Generic;
using Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.Menu
{
    public class MenuLevelsWindow : MonoBehaviour
    {
        [SerializeField] private MenuLevelButton _levelButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _backButton;

        private readonly List<MenuLevelButton> _buttons = new List<MenuLevelButton>();

        private ProgressService _progressService;
        private DataService _dataService;

        [Inject]
        public void Construct(ProgressService progressService, DataService dataService)
        {
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

            var availableLevels = lastPassedLevel == -1
                ? 1
                : lastPassedLevel + 2;
            availableLevels = Mathf.Min(availableLevels, _dataService.GetNumberOfLevels());

            for (var i = 0; i < availableLevels; i++)
            {
                var button = Instantiate(_levelButtonPrefab, _buttonsContainer);
                var levelData = _dataService.GetLevelData(i);
                button.Init(levelData);

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