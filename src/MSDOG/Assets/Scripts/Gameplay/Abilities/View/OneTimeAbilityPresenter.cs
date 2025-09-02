using Common;
using Core.Controllers;
using Core.Models.Data;
using Core.Services;
using Gameplay.Abilities.Core;
using Gameplay.Abilities.View.VFX;
using Gameplay.Factories;
using Gameplay.Interfaces;
using UnityEngine;
using UnityEngine.Assertions;

namespace Gameplay.Abilities.View
{
    public class OneTimeAbilityPresenter
    {
        private readonly IAbilityVFXFactory _abilityVFXFactory;
        private readonly ISoundController _soundController;
        private readonly IDataService _dataService;

        private readonly IEntityWithAbilities _entityWithAbilities;
        private readonly ICooldownAbility _ability;
        private readonly AbilityData _abilityData;

        public OneTimeAbilityPresenter(IEntityWithAbilities entityWithAbilities, ICooldownAbility ability,
            AbilityData abilityData, IAbilityVFXFactory abilityVFXFactory, ISoundController soundController,
            IDataService dataService)
        {
            Assert.IsNotNull(abilityData.FollowingAbilityVFXPrefab);

            _abilityVFXFactory = abilityVFXFactory;
            _soundController = soundController;
            _dataService = dataService;

            _entityWithAbilities = entityWithAbilities;
            _ability = ability;
            _abilityData = abilityData;

            ability.OnActionInvoked += OnAbilityActionInvoked;
            ability.OnDisposed += OnAbilityDisposed;
        }

        private void OnAbilityActionInvoked()
        {
            _abilityVFXFactory.CreateEffect<OneTimeAbilityVFX>(_entityWithAbilities, _abilityData);
            _soundController.PlayAbilityActivationSfx(_abilityData.ActivationSound);

            if (_dataService.GetSettings().ShowDebugHitboxes)
            {
                TryShowDebugEffect();
            }
        }

        private void OnAbilityDisposed()
        {
            _ability.OnActionInvoked -= OnAbilityActionInvoked;
            _ability.OnDisposed -= OnAbilityDisposed;
        }

        private void TryShowDebugEffect()
        {
            switch (_abilityData.AbilityType)
            {
                case AbilityType.CuttingBlow:
                    ShowCuttingBlowDebugEffect(_abilityData.Size);
                    break;
                case AbilityType.RoundAttack:
                    ShowRoundAttackDebugEffect(_abilityData.Size);
                    break;
            }
        }

        private void ShowCuttingBlowDebugEffect(float size)
        {
            var slashIndicator = GameObject.CreatePrimitive(PrimitiveType.Cube);
            slashIndicator.transform.position = _entityWithAbilities.GetPosition();
            slashIndicator.transform.rotation = Quaternion.identity;
            slashIndicator.transform.localScale = new Vector3(size, Constants.CuttingBlowAbility.BoxHeight,
                Constants.CuttingBlowAbility.BoxWidth);

            var renderer = slashIndicator.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.color = Color.red;

            Object.Destroy(slashIndicator.GetComponent<Collider>());
            Object.Destroy(slashIndicator, 0.2f);
        }

        private void ShowRoundAttackDebugEffect(float size)
        {
            var slashIndicator = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            slashIndicator.transform.position = _entityWithAbilities.GetPosition();
            slashIndicator.transform.rotation = Quaternion.identity;
            var diameter = size * 2;
            slashIndicator.transform.localScale = new Vector3(diameter, 0.5f, diameter);

            var renderer = slashIndicator.GetComponent<Renderer>();
            var mat = renderer.material;
            mat.color = Color.red;

            Object.Destroy(slashIndicator.GetComponent<Collider>());
            Object.Destroy(slashIndicator, 0.2f);
        }
    }
}