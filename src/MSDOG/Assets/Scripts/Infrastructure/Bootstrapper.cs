using Infrastructure.StateMachine;
using UnityEngine;
using VContainer;

namespace Infrastructure
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GlobalLifetimeScope _globalLifetimeScope;

        private void Awake()
        {
            _globalLifetimeScope.BuildContainer();

            var gameStateMachine = _globalLifetimeScope.Container.Resolve<GameStateMachine>();
            gameStateMachine.Enter<BootstrapState>();
        }
    }
}