using System.Collections.Generic;
using Constants;
using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Helpers;
using Gameplay.Services;
using UnityEngine;

namespace Gameplay.Abilities
{
    public class CuttingBlowAbility : BaseCooldownAbility
    {
        private const float BoxHeight = 2f;
        private const float BoxWidth = 1.7f;

        private readonly VfxFactory _vfxFactory;
        private readonly DataService _dataService;

        private readonly Player _player;
        private readonly int _damage;
        private readonly float _length;
        private readonly Collider[] _hitBuffer = new Collider[32];

        public CuttingBlowAbility(AbilityData abilityData, Player player, VfxFactory vfxFactory,
            DataService dataService, SoundService soundService)
            : base(abilityData, soundService)
        {
            _vfxFactory = vfxFactory;
            _dataService = dataService;

            _player = player;
            _damage = abilityData.Damage;
            _length = abilityData.Size;
        }

        protected override void InvokeAction()
        {
            Slash();

            _vfxFactory.CreateSlashEffect(_player.GetAbilitySpawnPosition(AbilityType), _length);

            if (_dataService.GetSettingsData().ShowDebugHitboxes)
            {
                ShowSlashDebugEffect();
            }
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

            var hits = Physics.OverlapBoxNonAlloc(boxCenter, boxSize * 0.5f, _hitBuffer, Quaternion.identity,
                Settings.LayerMasks.EnemyLayer);
            for (var i = 0; i < hits; i++)
            {
                var collider = _hitBuffer[i];
                if (collider.gameObject.TryGetComponentInHierarchy<Enemy>(out var enemy))
                {
                    hitEnemies.Add(enemy);
                }
            }

            return hitEnemies;
        }

        private void ShowSlashDebugEffect()
        {
            var slashIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slashIndicator.transform.position = _player.transform.position;
            slashIndicator.transform.rotation = Quaternion.identity;
            slashIndicator.transform.localScale = new Vector3(_length, BoxHeight, BoxWidth);

            var renderer = slashIndicator.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.color = Color.red;

            Object.Destroy(slashIndicator.GetComponent<Collider>());
            Object.Destroy(slashIndicator, 0.2f);
        }
    }
}