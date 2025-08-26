using System;
using Gameplay.Services;
using UnityEngine;
using VContainer;

namespace Utility
{
    public class AnimatorSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IGameSpeedService _gameSpeedService;

        [Inject]
        public void Construct(IGameSpeedService gameSpeedService)
        {
            _gameSpeedService = gameSpeedService;
        }

        private void OnEnable()
        {
            _gameSpeedService.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _animator.speed = _gameSpeedService.GameSpeed;
        }

        private void OnDisable()
        {
            _gameSpeedService.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}