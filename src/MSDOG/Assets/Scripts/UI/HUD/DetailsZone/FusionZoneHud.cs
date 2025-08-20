using Core.Controllers;
using Core.Sounds;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class FusionZoneHud : MonoBehaviour
    {
        [SerializeField] private FusionSlotHud _fusionSlotHud1;
        [SerializeField] private FusionSlotHud _fusionSlotHud2;
        [SerializeField] private Button _upgradeButton;

        private ISoundController _soundController;
        private IDetailService _detailService;

        [Inject]
        public void Construct(ISoundController soundController, IDetailService detailService)
        {
            _detailService = detailService;
            _soundController = soundController;
        }

        private void OnEnable()
        {
            _upgradeButton.gameObject.SetActive(false);
            _upgradeButton.onClick.AddListener(UpgradeAbility);
        }

        private void OnDisable()
        {
            _upgradeButton.onClick.RemoveListener(UpgradeAbility);
        }

        public void OnDetailEntersTheZone()
        {
            var detailPart1 = _fusionSlotHud1.DetailPart;
            var detailPart2 = _fusionSlotHud2.DetailPart;

            if (!detailPart1 || !detailPart2)
            {
                return;
            }

            var abilityData1 = detailPart1.AbilityData;
            var abilityData2 = detailPart2.AbilityData;

            if (abilityData1.AbilityType != abilityData2.AbilityType)
            {
                return;
            }

            if (abilityData1.Level != abilityData2.Level)
            {
                return;
            }

            var hasUpgrade = _detailService.TryGetUpgrade(detailPart1.Detail, out _);
            _upgradeButton.gameObject.SetActive(hasUpgrade);
        }

        public void OnDetailExitsTheZone()
        {
            _upgradeButton.gameObject.SetActive(false);
        }

        private void UpgradeAbility()
        {
            _detailService.TryGetUpgrade(_fusionSlotHud1.DetailPart.Detail, out var upgradedAbilityData);

            _detailService.CreateInactiveDetail(upgradedAbilityData);
            _fusionSlotHud1.DestroyDetail();
            _fusionSlotHud2.DestroyDetail();

            _soundController.PlaySfx(SfxType.Fusion);
        }
    }
}