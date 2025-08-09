using Core.Controllers;
using Core.Services;
using Core.Sounds;
using Gameplay.Services;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class DestructZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        private IDataService _dataService;
        private ISoundController _soundController;
        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(IPlayerProvider playerProvider, IDataService dataService, ISoundController soundController)
        {
            _playerProvider = playerProvider;
            _soundController = soundController;
            _dataService = dataService;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            if (!eventData.pointerDrag.gameObject.TryGetComponent<DetailPartHud>(out var detailPart))
            {
                return;
            }

            detailPart.SetCurrentZone(this);
        }

        public void Enter(DetailPartHud detailPart)
        {
            var player = _playerProvider.Player;
            var settingsData = _dataService.GetSettingsData();

            if (player.IsFullHealth)
            {
                player.CollectExperience(settingsData.ExperiencePerDestructedDetail);
            }
            else
            {
                player.Heal(settingsData.HealPerDestructedDetail);
            }

            detailPart.Destruct();

            _soundController.PlaySfx(SfxType.Destructor);
        }

        public void Exit(DetailPartHud detailPart)
        {
            Debug.LogError("Can't exit from that zone!");
        }
    }
}