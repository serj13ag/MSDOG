using System;
using Gameplay.Controllers;
using UnityEngine;
using VContainer;

namespace Utility
{
    public class AnimatorSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IGameplayUpdateController _updateController;

        [Inject]
        public void Construct(IGameplayUpdateController updateController)
        {
            _updateController = updateController;
        }

        private void OnEnable()
        {
            _updateController.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _animator.speed = _updateController.GameTimeScale;
        }

        private void OnDisable()
        {
            _updateController.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}