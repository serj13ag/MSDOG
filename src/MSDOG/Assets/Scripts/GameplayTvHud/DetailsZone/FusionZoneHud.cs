using Core.Controllers;
using Core.Sounds;
using GameplayTvHud.Mediators;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace GameplayTvHud.DetailsZone
{
    public class FusionZoneHud : MonoBehaviour
    {
        [SerializeField] private FusionSlotHud _fusionSlotHud1;
        [SerializeField] private FusionSlotHud _fusionSlotHud2;
        [SerializeField] private Button _upgradeButton;

        private ISoundController _soundController;
        private IDetailMediator _detailMediator;

        [Inject]
        public void Construct(ISoundController soundController, IDetailMediator detailMediator)
        {
            _detailMediator = detailMediator;
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
            var detailView1 = _fusionSlotHud1.DetailView;
            var detailView2 = _fusionSlotHud2.DetailView;

            if (!detailView1 || !detailView2)
            {
                return;
            }

            var abilityData1 = detailView1.Detail.AbilityData;
            var abilityData2 = detailView2.Detail.AbilityData;

            if (abilityData1.AbilityType != abilityData2.AbilityType)
            {
                return;
            }

            if (abilityData1.Level != abilityData2.Level)
            {
                return;
            }

            var hasUpgrade = _detailMediator.HasUpgrade(detailView1.Detail);
            _upgradeButton.gameObject.SetActive(hasUpgrade);
        }

        public void OnDetailExitsTheZone()
        {
            _upgradeButton.gameObject.SetActive(false);
        }

        private void UpgradeAbility()
        {
            _detailMediator.UpgradeDetail(_fusionSlotHud1.DetailView.Detail);
            
            _fusionSlotHud1.DestroyDetail();
            _fusionSlotHud2.DestroyDetail();

            _soundController.PlaySfx(SfxType.Fusion);
        }
    }
}