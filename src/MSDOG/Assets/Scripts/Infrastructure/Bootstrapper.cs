using Infrastructure.StateMachine;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GlobalServicesScope _globalServicesScope;

        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            _globalServicesScope.BuildContainer();
            _globalServicesScope.Container.Resolve<GameStateMachine>().Enter<BootstrapState>();
        }
    }
}