using System.Collections.Generic;
using Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MenuLevelsWindow : MonoBehaviour
    {
        [SerializeField] private MenuLevelButton _levelButtonPrefab;
        [SerializeField] private Transform _buttonsContainer;
        [SerializeField] private Button _backButton;

        private readonly List<MenuLevelButton> _buttons = new List<MenuLevelButton>();

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
            var progressService = GlobalServices.ProgressService;
            var dataService = GlobalServices.DataService;

            var lastPassedLevel = progressService.LastPassedLevel;

            var availableLevels = lastPassedLevel == -1
                ? 1
                : lastPassedLevel + 2;
            availableLevels = Mathf.Min(availableLevels, dataService.GetNumberOfLevels());

            for (var i = 0; i < availableLevels; i++)
            {
                var button = Instantiate(_levelButtonPrefab, _buttonsContainer);
                var levelData = dataService.GetLevelData(i);
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