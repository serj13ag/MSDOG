using Data;
using Services;
using Sounds;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class FusionZoneHud : MonoBehaviour
    {
        [SerializeField] private DetailsZoneHud _detailsZoneHud;
        [SerializeField] private FusionSlotHud _fusionSlotHud1;
        [SerializeField] private FusionSlotHud _fusionSlotHud2;
        [SerializeField] private Button _upgradeButton;

        private DataService _dataService;
        private SoundService _soundService;

        private AbilityData _upgradedAbilityData;

        [Inject]
        public void Construct(DataService dataService, SoundService soundService)
        {
            _soundService = soundService;
            _dataService = dataService;
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

            var hasUpgrade =
                _dataService.TryGetAbilityUpgradeData(abilityData1.AbilityType, abilityData1.Level, out _upgradedAbilityData);

            _upgradeButton.gameObject.SetActive(hasUpgrade);
        }

        public void OnDetailExitsTheZone()
        {
            _upgradedAbilityData = null;
            _upgradeButton.gameObject.SetActive(false);
        }

        private void UpgradeAbility()
        {
            _detailsZoneHud.CreateDetail(_upgradedAbilityData);
            _fusionSlotHud1.DestroyDetail();
            _fusionSlotHud2.DestroyDetail();

            _soundService.PlaySfx(SfxType.Fusion);
        }
    }
}