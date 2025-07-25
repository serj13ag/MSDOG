using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GlobalServicesScope _globalServicesScope;

        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine();
            _globalServicesScope.SetGameStateMachine(_gameStateMachine);
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}