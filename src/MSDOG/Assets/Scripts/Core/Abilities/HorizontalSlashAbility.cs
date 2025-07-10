using System.Collections.Generic;
using Core.Enemies;
using UnityEngine;

namespace Core.Abilities
{
    public class HorizontalSlashAbility
    {
        private const float BoxHeight = 2f;
        private const float BoxWidth = 1f;

        private readonly int _enemyLayerMask = LayerMask.GetMask("Enemy");

        private readonly Player _player;
        private readonly float _cooldown;
        private readonly int _damage;
        private readonly float _length;
        private readonly Collider[] _hitBuffer = new Collider[32];

        private float _timeTillSlash;

        public HorizontalSlashAbility(Player player)
        {
            _player = player;
            _cooldown = 2f;
            _damage = 1;
            _length = 7f;

            ResetTimeTillSlash();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_timeTillSlash > 0f)
            {
                _timeTillSlash -= deltaTime;
                return;
            }

            Slash();
            ShowSlashEffect();
            ResetTimeTillSlash();
        }

        private void Slash()
        {
            var hitEnemies = DetectEnemiesInStrikeBox();
            foreach (var enemy in hitEnemies)
            {
                enemy.TakeDamage(_damage);
            }
        }

        private List<Enemy> DetectEnemiesInStrikeBox()
        {
            var hitEnemies = new List<Enemy>();

            var boxCenter = _player.transform.position;
            var boxSize = new Vector3(_length, BoxHeight, BoxWidth);

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize * 0.5f, _hitBuffer, Quaternion.identity, _enemyLayerMask);
            for (var i = 0; i < hits; i++)
            {
                var collider = _hitBuffer[i];
                if (collider.TryGetComponent<Enemy>(out var enemy))
                {
                    hitEnemies.Add(enemy);
                }
            }

            return hitEnemies;
        }

        private void ResetTimeTillSlash()
        {
            _timeTillSlash = _cooldown;
        }

        private void ShowSlashEffect()
        {
            var slashIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slashIndicator.transform.position = _player.transform.position;
            slashIndicator.transform.rotation = Quaternion.identity;
            slashIndicator.transform.localScale = new Vector3(_length, BoxHeight, BoxWidth);

            var renderer = slashIndicator.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.color = Color.red;

            Object.Destroy(slashIndicator.GetComponent<Collider>());
            Object.Destroy(slashIndicator, 0.5f);
        }
    }
}