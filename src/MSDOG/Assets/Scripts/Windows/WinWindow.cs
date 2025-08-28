using Core.Services;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class WinWindow : BaseWindow
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _toNextLevelButton;

        private IGameStateMachine _gameStateMachine;
        private IDataService _dataService;
        private ILevelFlowService _levelFlowService;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, IDataService dataService, ILevelFlowService levelFlowService)
        {
            _gameStateMachine = gameStateMachine;
            _dataService = dataService;
            _levelFlowService = levelFlowService;

            _toNextLevelButton.gameObject.SetActive(!levelFlowService.IsLastLevel);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.AddListener(OnToNextLevelButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _toNextLevelButton.onClick.RemoveListener(OnToNextLevelButtonClicked);
        }

        private void OnToNextLevelButtonClicked()
        {
            var nextLevelIndex = _levelFlowService.CurrentLevelIndex + 1;
            if (nextLevelIndex < _dataService.GetNumberOfLevels())
            {
                _gameStateMachine.Enter<GameplayState, int>(nextLevelIndex);
            }
            else
            {
                Debug.LogWarning("This is last level! Going to main menu.");
                _gameStateMachine.Enter<MainMenuState>();
            }

            Close();
        }

        private void OnToMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<MainMenuState>();
            Close();
        }
    }
}