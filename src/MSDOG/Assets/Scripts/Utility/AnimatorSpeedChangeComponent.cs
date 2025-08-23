using System;
using Core.Controllers;
using UnityEngine;
using VContainer;

namespace Utility
{
    public class AnimatorSpeedChangeComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IUpdateController _updateController;

        [Inject]
        public void Construct(IUpdateController updateController)
        {
            _updateController = updateController;

            updateController.OnGameTimeChanged += OnGameTimeChanged;
        }

        private void OnGameTimeChanged(object sender, EventArgs e)
        {
            _animator.speed = _updateController.GameTimeScale;
        }

        private void OnDestroy()
        {
            _updateController.OnGameTimeChanged -= OnGameTimeChanged;
        }
    }
}