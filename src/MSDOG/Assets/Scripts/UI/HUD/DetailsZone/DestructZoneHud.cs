using Core.Controllers;
using Core.Services;
using Core.Sounds;
using Gameplay.Factories;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace UI.HUD.DetailsZone
{
    public class DestructZoneHud : MonoBehaviour, IDetailsZone, IDropHandler
    {
        private GameFactory _gameFactory;
        private DataService _dataService;
        private SoundController _soundController;

        [Inject]
        public void Construct(GameFactory gameFactory, DataService dataService, SoundController soundController)
        {
            _soundController = soundController;
            _dataService = dataService;
            _gameFactory = gameFactory;
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
            var player = _gameFactory.Player;
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