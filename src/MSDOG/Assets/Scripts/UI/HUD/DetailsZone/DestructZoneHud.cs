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
        private PlayerService _playerService;

        [Inject]
        public void Construct(PlayerService playerService, IDataService dataService, ISoundController soundController)
        {
            _playerService = playerService;
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
            var player = _playerService.Player;
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