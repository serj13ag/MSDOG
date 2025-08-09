using System.Collections.Generic;
using Constants;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Enemies;
using Gameplay.Factories;
using UnityEngine;
using Utility;

namespace Gameplay.Abilities
{
    public class RoundAttackAbility : BaseCooldownAbility
    {
        private readonly VfxFactory _vfxFactory;
        private readonly IDataService _dataService;

        private readonly Player _player;
        private readonly int _damage;
        private readonly float _radius;
        private readonly Collider[] _hitBuffer = new Collider[32];

        public RoundAttackAbility(AbilityData abilityData, Player player, VfxFactory vfxFactory,
            IDataService dataService, ISoundController soundController)
            : base(abilityData, soundController)
        {
            _vfxFactory = vfxFactory;
            _dataService = dataService;

            _player = player;
            _damage = abilityData.Damage;
            _radius = abilityData.Size;
        }

        protected override void InvokeAction()
        {
            Slash();

            _vfxFactory.CreateRoundAttackEffect(_player.GetAbilitySpawnPosition(AbilityType), _radius);

            if (_dataService.GetSettingsData().ShowDebugHitboxes)
            {
                ShowDebugEffect();
            }
        }

        private void Slash()
        {
            var hitEnemies = DetectEnemiesInRadius();
            foreach (var enemy in hitEnemies)
            {
                enemy.TakeDamage(_damage);
            }
        }

        private List<Enemy> DetectEnemiesInRadius()
        {
            var hitEnemies = new List<Enemy>();

            var circleCenter = _player.transform.position;
            var hits = Physics.OverlapSphereNonAlloc(circleCenter, _radius, _hitBuffer, Settings.LayerMasks.EnemyLayer);
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

        private void ShowDebugEffect()
        {
            var slashIndicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            slashIndicator.transform.position = _player.transform.position;
            slashIndicator.transform.rotation = Quaternion.identity;
            var diameter = _radius * 2;
            slashIndicator.transform.localScale = new Vector3(diameter, 0.5f, diameter);

            var renderer = slashIndicator.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.color = Color.red;

            Object.Destroy(slashIndicator.GetComponent<Collider>());
            Object.Destroy(slashIndicator, 0.2f);
        }
    }
}