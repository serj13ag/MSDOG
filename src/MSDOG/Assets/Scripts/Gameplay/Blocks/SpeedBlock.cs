using Gameplay.Interfaces;
using UnityEngine;

namespace Gameplay.Blocks
{
    public class SpeedBlock
    {
        private readonly IEntityWithMoveSpeed _entityWithMoveSpeed;

        private bool _movementIsActive;
        private float _additionalMoveSpeed;
        private float _nitroMultiplier = 1f;

        public bool HasNitro => _nitroMultiplier > 1f;

        public SpeedBlock(IEntityWithMoveSpeed entityWithMoveSpeed)
        {
            _entityWithMoveSpeed = entityWithMoveSpeed;
        }

        public void SetActive(bool value)
        {
            _movementIsActive = value;
        }

        public void ChangeAdditionalSpeed(float speed)
        {
            var newAdditionalSpeed = _additionalMoveSpeed + speed;
            newAdditionalSpeed = Mathf.Max(newAdditionalSpeed, 0);
            _additionalMoveSpeed = newAdditionalSpeed;
        }

        public void SetNitro(float nitroMultiplier)
        {
            _nitroMultiplier = nitroMultiplier;
        }

        public void ResetNitro()
        {
            _nitroMultiplier = 1f;
        }

        public float GetCurrentMoveSpeed()
        {
            if (!_movementIsActive)
            {
                return 0f;
            }

            return (_entityWithMoveSpeed.BaseMoveSpeed + _additionalMoveSpeed) * _nitroMultiplier;
        }
    }
}