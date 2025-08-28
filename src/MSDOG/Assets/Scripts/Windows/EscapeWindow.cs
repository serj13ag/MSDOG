using Core.Controllers;
using Gameplay.Services;
using Infrastructure.StateMachine;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Windows
{
    public class EscapeWindow : BaseCloseableWindow
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _menuButton;

        private IGameStateMachine _gameStateMachine;
        private ILevelFlowService _levelFlowService;
        private IWindowController _windowController;

        [Inject]
        public void Construct(IGameStateMachine gameStateMachine, ILevelFlowService levelFlowService,
            IWindowController windowController)
        {
            _windowController = windowController;
            _levelFlowService = levelFlowService;
            _gameStateMachine = gameStateMachine;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            _restartButton.onClick.AddListener(Restart);
            _optionsButton.onClick.AddListener(OnOptionsButtonClicked);
            _menuButton.onClick.AddListener(GoToMenu);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            _restartButton.onClick.RemoveListener(Restart);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClicked);
            _menuButton.onClick.RemoveListener(GoToMenu);
        }

        private void Restart()
        {
            _gameStateMachine.Enter<GameplayState, int>(_levelFlowService.CurrentLevelIndex);
        }

        private void GoToMenu()
        {
            _gameStateMachine.Enter<MainMenuState>();
        }

        private void OnOptionsButtonClicked()
        {
            _windowController.ShowOptionsWindow();
        }
    }
}