using Infrastructure.StateMachine;
using UnityEngine;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _gameStateMachine = new GameStateMachine();
            _gameStateMachine.Enter<BootstrapState>();
        }
    }
}