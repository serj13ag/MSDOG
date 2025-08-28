using System.Collections.Generic;
using Common;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.AbilityEffects;
using Gameplay.Enemies;
using Gameplay.Factories;
using UnityEngine;
using Utility.Extensions;

namespace Gameplay.Abilities
{
    public class CuttingBlowAbility : BaseCooldownAbility
    {
        private const float BoxHeight = 2f;
        private const float BoxWidth = 1.7f;

        private readonly IAbilityEffectFactory _abilityEffectFactory;
        private readonly IDataService _dataService;

        private readonly AbilityData _abilityData;
        private readonly Player _player;
        private readonly int _damage;
        private readonly float _length;
        private readonly Collider[] _hitBuffer = new Collider[32];

        public CuttingBlowAbility(AbilityData abilityData, Player player, IAbilityEffectFactory abilityEffectFactory,
            IDataService dataService, ISoundController soundController)
            : base(abilityData, soundController)
        {
            _abilityEffectFactory = abilityEffectFactory;
            _dataService = dataService;

            _abilityData = abilityData;
            _player = player;
            _damage = abilityData.Damage;
            _length = abilityData.Size;
        }

        protected override void InvokeAction()
        {
            Slash();

            _abilityEffectFactory.CreateEffect<OneTimeAbilityEffect>(_player, _abilityData);

            if (_dataService.GetSettings().ShowDebugHitboxes)
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
                Constants.LayerMasks.EnemyLayer);
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