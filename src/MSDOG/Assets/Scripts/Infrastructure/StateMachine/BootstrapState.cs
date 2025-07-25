using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class BootstrapState : IState
    {
        private readonly GameStateMachine _gameStateMachine;

        public BootstrapState(GameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
        }

        public void Enter()
        {
            var globalScope = Object.FindFirstObjectByType<GlobalServicesScope>();
            globalScope.BuildContainer();

            GlobalServices.Initialize(globalScope.Container);
            _gameStateMachine.ResolveStates();
            _gameStateMachine.Enter<MainMenuState>();
        }

        public void Exit()
        {
        }
    }
}