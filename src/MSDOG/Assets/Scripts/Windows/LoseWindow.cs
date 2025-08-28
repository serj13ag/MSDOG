using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class LoseWindow : BaseWindow
    {
        [SerializeField] private Button _toMainMenuButton;
        [SerializeField] private Button _restartLevelButton;

        private IGameStateMachine _gameStateMachine;
        private ILevelFlowService _levelFlowService;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, ILevelFlowService levelFlowService)
        {
            _gameStateMachine = gameStateMachine;
            _levelFlowService = levelFlowService;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _toMainMenuButton.onClick.AddListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.AddListener(OnRestartLevelButtonClicked);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _toMainMenuButton.onClick.RemoveListener(OnToMainMenuButtonClicked);
            _restartLevelButton.onClick.RemoveListener(OnRestartLevelButtonClicked);
        }

        private void OnRestartLevelButtonClicked()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelFlowService.CurrentLevelIndex);
            Close();
        }

        private void OnToMainMenuButtonClicked()
        {
            _gameStateMachine.Enter<MainMenuState>();
            Close();
        }
    }
}